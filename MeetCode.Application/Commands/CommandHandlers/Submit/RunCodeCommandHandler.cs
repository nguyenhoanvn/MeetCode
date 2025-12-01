using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Submit;
using MeetCode.Application.Commands.CommandResults.Submit;
using MeetCode.Application.DTOs.Other;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Exceptions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.Submit
{
    public class RunCodeCommandHandler : IRequestHandler<RunCodeCommand, Result<RunCodeCommandResult>>
    {
        private readonly ISubmitService _submitService;
        private readonly ILogger<RunCodeCommandHandler> _logger;
        private readonly IProblemService _problemService;
        private readonly ILanguageService _languageService;
        private readonly ITestCaseService _testCaseService;
        public RunCodeCommandHandler(
            ISubmitService submitService,
            IProblemService problemService,
            ILanguageService languageService,
            ITestCaseService testCaseService,
            ILogger<RunCodeCommandHandler> logger)
        {
            _submitService = submitService;
            _problemService = problemService;
            _languageService = languageService;
            _testCaseService = testCaseService;
            _logger = logger;
        }
        public async Task<Result<RunCodeCommandResult>> Handle(RunCodeCommand request, CancellationToken ct)
        {
            var problem = await _problemService.FindProblemByIdAsync(request.ProblemId, ct);
            if (problem == null)
            {
                _logger.LogWarning($"Cannot find problem with Id {request.ProblemId}");
                throw new EntityNotFoundException<MeetCode.Domain.Entities.Problem>(nameof(request.ProblemId), request.ProblemId.ToString());
            }

            var language = await _languageService.FindLanguageByIdAsync(request.LanguageId, ct);
            if (language == null)
            {
                _logger.LogWarning($"Cannot find language with Id {request.LanguageId}");
                throw new EntityNotFoundException<MeetCode.Domain.Entities.Language>(nameof(request.LanguageId), request.ProblemId.ToString())
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
                    resultList.Add(await _submitService.RunCodeAsync(request.Code, language, problem, testCase, ct));
                }
                
                var result = new RunCodeCommandResult(resultList);
                return Result.Success(result);
            } catch (Exception ex)
            {
                return Result.Error("Failed to enqueue message:" + ex.Message);
            }
        }
    }
}
