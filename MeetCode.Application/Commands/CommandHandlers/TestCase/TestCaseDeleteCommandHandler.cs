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
    public class TestCaseDeleteCommandHandler : IRequestHandler<TestCaseDeleteCommand, Result<TestCaseDeleteCommandResult>>
    {
        private readonly ILogger<TestCaseDeleteCommandHandler> _logger;
        private readonly ITestCaseService _testCaseService;
        public TestCaseDeleteCommandHandler(
            ILogger<TestCaseDeleteCommandHandler> logger,
            ITestCaseService testCaseService
            )
        {
            _logger = logger;
            _testCaseService = testCaseService;
        }
        public async Task<Result<TestCaseDeleteCommandResult>> Handle(TestCaseDeleteCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to delete test case with Id {request.TestCaseId}");

            var testCase = await _testCaseService.FindTestCaseByIdAsync(request.TestCaseId, ct);
            if (testCase == null)
            {
                _logger.LogWarning($"Delete failed because cannot found test case with Id {request.TestCaseId}");
                return Result.Error($"Delete failed because cannot found test case with Id {request.TestCaseId}");
            }

            await _testCaseService.DeleteTestCaseAsync(testCase, ct);
            _logger.LogInformation($"Delete test case with Id {request.TestCaseId} successfully");

            var result = new TestCaseDeleteCommandResult($"Delete test case with Id {request.TestCaseId} successfully");
            return result;
        }
    }
}
