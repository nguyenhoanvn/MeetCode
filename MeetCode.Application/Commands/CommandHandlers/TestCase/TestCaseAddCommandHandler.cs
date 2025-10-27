using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.TestCase;
using MeetCode.Application.Commands.CommandResults.TestCase;
using MeetCode.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Commands.CommandHandlers.TestCase
{
    public class TestCaseAddCommandHandler : IRequestHandler<TestCaseAddCommand, Result<TestCaseAddCommandResult>>
    {
        private readonly ILogger<TestCaseAddCommandHandler> _logger;
        private readonly ITestCaseService _testCaseService;
        public TestCaseAddCommandHandler(
            ILogger<TestCaseAddCommandHandler> logger,
            ITestCaseService testCaseService)
        {
            _logger = logger;
            _testCaseService = testCaseService;
        }
        public async Task<Result<TestCaseAddCommandResult>> Handle(TestCaseAddCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to add new test case with input {request.InputText}, output {request.ExpectedOutputText}, and problem {request.ProblemId}");
            if (request == null)
            {
                _logger.LogWarning($"Add new test case failed because request is null");
                return Result.Error($"Add new test case failed because request is null");
            }

            var testCase = await _testCaseService.CreateTestCaseAsync(
                request.Visibility,
                request.InputText,
                request.ExpectedOutputText,
                request.Weight,
                request.ProblemId,
                ct);

            _logger.LogInformation($"Test case with input {request.InputText}, output {request.ExpectedOutputText}, and problem {request.ProblemId} added successfully");
            var result = new TestCaseAddCommandResult(testCase);
            return Result.Success(result);
        }
    }
}
