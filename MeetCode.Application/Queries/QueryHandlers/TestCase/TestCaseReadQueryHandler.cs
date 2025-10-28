using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.TestCase;
using MeetCode.Application.Queries.QueryResults.TestCase;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Queries.QueryHandlers.TestCase
{
    public class TestCaseReadQueryHandler : IRequestHandler<TestCaseReadQuery, Result<TestCaseReadQueryResult>>
    {
        private readonly ILogger<TestCaseReadQueryHandler> _logger;
        private readonly ITestCaseService _testCaseService;
        public TestCaseReadQueryHandler(
            ILogger<TestCaseReadQueryHandler> logger,
            ITestCaseService testCaseService
            )
        {
            _logger = logger;
            _testCaseService = testCaseService;
        }
        public async Task<Result<TestCaseReadQueryResult>> Handle(TestCaseReadQuery request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to read test case {request.TestId}");

            var testCase = await _testCaseService.FindTestCaseByIdAsync(request.TestId, ct);
            if (testCase == null)
            {
                _logger.LogWarning($"Test case {request.TestId} does not exists");
                return Result.NotFound($"Test case {request.TestId} does not exists");
            }

            var result = new TestCaseReadQueryResult(testCase);
            return Result.Success(result);
        }
    }
}
