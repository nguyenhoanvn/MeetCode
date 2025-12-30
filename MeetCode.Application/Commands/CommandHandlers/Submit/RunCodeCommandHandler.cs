using Ardalis.Result;
using Fleck;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Submit;
using MeetCode.Application.Commands.CommandResults.Submit;
using MeetCode.Application.DTOs.Other;
using MeetCode.Application.Interfaces.Messagings;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.Submit
{
    public class RunCodeCommandHandler : IRequestHandler<RunCodeCommand, Result<RunCodeCommandResult>>
    {
        private readonly ISubmitService _submitService;
        private readonly ILogger<RunCodeCommandHandler> _logger;
        private readonly IProblemTemplateService _problemTemplateService;
        private readonly ISubmissionRepository _submissionRepository;
        private readonly IProblemRepository _problemRepository;
        private readonly ILanguageService _languageService;
        private readonly ITestCaseService _testCaseService;
        private readonly IUnitOfWork _unitOfWork;
        public RunCodeCommandHandler(
            ISubmitService submitService,
            IProblemTemplateService problemTemplateService,
            ISubmissionRepository submissionRepository,
            IProblemRepository problemRepository,
            ILanguageService languageService,
            ITestCaseService testCaseService,
            IUnitOfWork unitOfWork,
            ILogger<RunCodeCommandHandler> logger)
        {
            _submitService = submitService;
            _problemTemplateService = problemTemplateService;
            _submissionRepository = submissionRepository;
            _problemRepository = problemRepository;
            _languageService = languageService;
            _testCaseService = testCaseService;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<RunCodeCommandResult>> Handle(RunCodeCommand request, CancellationToken ct)
        {
            var template = await _problemTemplateService.FindTemplateByProblemIdLanguageNameAsync(request.ProblemId, request.LanguageName, ct);
            if (template == null)
            {
                _logger.LogWarning($"Cannot find template with ProblemId {request.ProblemId} and LanguageName {request.LanguageName}");
                throw new EntityNotFoundException("Problem", nameof(request.ProblemId), request.ProblemId.ToString());
            }

            var language = await _languageService.FindLanguageByNameAsync(request.LanguageName, ct);
            if (language == null)
            {
                _logger.LogWarning($"Cannot find language with name {request.LanguageName}");
                throw new EntityNotFoundException("Language", nameof(request.LanguageName), request.ProblemId.ToString());
            }

            var testCases = (await _testCaseService.FindTestCaseByIdsAsync(request.TestCaseIds, ct)).ToList();
            if (testCases.Count() == 0)
            {
                _logger.LogWarning("Test cases are empty");
                return Result.Success();
            }

            try
            {
                var resultList = new List<TestResult>();
                foreach (var testCase in testCases)
                {
                    resultList.Add(await _submitService.RunCodeAsync(request.Code, language, template, testCase, ct));
                }

                bool isAccepted = _submitService.IsSubmissionAccepted(resultList);


                var submission = new Submission
                {
                    UserId = request.UserId,
                    ProblemId = request.ProblemId,
                    LangId = language.LangId,
                    Verdict = isAccepted ? "accepted" : "wrong_answer",
                    SourceCode = request.Code,
                    ExecTimeMs = (int) resultList.MinBy(x => x.ExecTimeMs).ExecTimeMs
                };

                await _unitOfWork.BeginTransactionAsync(ct);

                await _submissionRepository.AddAsync(submission, ct);
                var problem = await _problemRepository.GetByIdAsync(request.ProblemId, ct);

                if (problem == null)
                {
                    _logger.LogWarning("Cannot find problem {ProblemId}", request.ProblemId);
                    await _unitOfWork.RollbackTransactionAsync(ct);
                    return Result.Invalid(new ValidationError(nameof(request.ProblemId), $"Cannot find problem {request.ProblemId}"));
                }

                problem.UpdateSubmission(submission.Verdict);

                await _unitOfWork.SaveChangesAsync(ct);
                await _unitOfWork.CommitTransactionAsync(ct);

                var result = new RunCodeCommandResult(request.JobId, "Completed", resultList);

                return Result.Success(result);
            } catch (Exception ex)
            {
                await _unitOfWork.RollbackTransactionAsync(ct);
                return Result.Error("Failed to enqueue message:" + ex.Message);
            }
        }
    }
}
