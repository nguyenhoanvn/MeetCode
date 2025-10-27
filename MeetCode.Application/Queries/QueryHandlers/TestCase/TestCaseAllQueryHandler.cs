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
    public class TestCaseAllQueryHandler : IRequestHandler<TestCaseAllQuery, Result<TestCaseAllQueryResult>>
    {
        private readonly ILogger<TestCaseAllQueryHandler> _logger;
        private readonly ITestCaseService _testCaseService;
        public TestCaseAllQueryHandler(
            ILogger<TestCaseAllQueryHandler> logger,
            ITestCaseService testCaseService
            )
        {
            _logger = logger;
            _testCaseService = testCaseService;
        }
        public async Task<Result<TestCaseAllQueryResult>> Handle(TestCaseAllQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to retrieve all test cases");
            var testCaseList = await _testCaseService.ReadAllTestCasesAsync(ct);
            if (testCaseList.Count() == 0)
            {
                _logger.LogWarning("Cannot find any test case");
            }
            var result = new TestCaseAllQueryResult(testCaseList);
            return Result.Success(result);
        }
    }
}
