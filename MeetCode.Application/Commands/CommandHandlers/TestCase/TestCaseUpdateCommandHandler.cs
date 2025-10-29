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
    public class TestCaseUpdateCommandHandler : IRequestHandler<TestCaseUpdateCommand, Result<TestCaseUpdateCommandResult>>
    {
        private readonly ILogger<TestCaseUpdateCommandHandler> _logger;
        private readonly ITestCaseService _testCaseService;
        public TestCaseUpdateCommandHandler(
            ILogger<TestCaseUpdateCommandHandler> logger,
            ITestCaseService testCaseService
            )
        {
            _logger = logger;
            _testCaseService = testCaseService;
        }

        public async Task<Result<TestCaseUpdateCommandResult>> Handle(TestCaseUpdateCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to update test case {request.TestCaseId}");

            var updatedTestCase = await _testCaseService.UpdateTestCaseAsync(
                request.TestCaseId,
                request.NewVisibility,
                request.NewInputText, 
                request.NewExpectedOutputText, 
                request.NewWeight,
                ct);

            _logger.LogInformation($"");
            var result = new TestCaseUpdateCommandResult(updatedTestCase);
            return result;

        }
    }
}
