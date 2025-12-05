using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Submit;
using MeetCode.Application.Commands.CommandResults.Submit;
using MeetCode.Application.DTOs.Other;
using MeetCode.Application.Interfaces.Messagings;
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
        private readonly IProblemTemplateService _problemTemplateService;
        private readonly ILanguageService _languageService;
        private readonly ITestCaseService _testCaseService;
        private readonly IJobWebSocketRegistry _ws;
        public RunCodeCommandHandler(
            ISubmitService submitService,
            IProblemTemplateService problemTemplateService,
            ILanguageService languageService,
            ITestCaseService testCaseService,
            IJobWebSocketRegistry ws,
            ILogger<RunCodeCommandHandler> logger)
        {
            _submitService = submitService;
            _problemTemplateService = problemTemplateService;
            _languageService = languageService;
            _testCaseService = testCaseService;
            _ws = ws;
            _logger = logger;
        }
        public async Task<Result<RunCodeCommandResult>> Handle(RunCodeCommand request, CancellationToken ct)
        {
            var template = await _problemTemplateService.FindTemplateByProblemIdLanguageIdAsync(request.ProblemId, request.LanguageId, ct);
            if (template == null)
            {
                _logger.LogWarning($"Cannot find template with ProblemId {request.ProblemId} and LanguageId {request.LanguageId}");
                throw new EntityNotFoundException("Problem", nameof(request.ProblemId), request.ProblemId.ToString());
            }

            var language = await _languageService.FindLanguageByIdAsync(request.LanguageId, ct);
            if (language == null)
            {
                _logger.LogWarning($"Cannot find language with Id {request.LanguageId}");
                throw new EntityNotFoundException("Language", nameof(request.LanguageId), request.ProblemId.ToString());
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
                
                var result = new RunCodeCommandResult(request.JobId, "Completed", resultList);

                await _ws.SendToJobAsync(request.JobId, result);
                return Result.Success(result);
            } catch (Exception ex)
            {
                return Result.Error("Failed to enqueue message:" + ex.Message);
            }
        }
    }
}
