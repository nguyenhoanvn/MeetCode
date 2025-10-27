using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using MeetCode.Domain.Exceptions;
using Microsoft.Extensions.Logging;

namespace MeetCode.Infrastructure.Services
{
    public class TestCaseService : ITestCaseService
    {
        private readonly ITestCaseRepository _testCaseRepository;
        private readonly ILogger<TestCaseService> _logger;
        private readonly IUnitOfWork _unitOfWork;
        public TestCaseService(
            ITestCaseRepository testCaseRepository,
            ILogger<TestCaseService> logger,
            IUnitOfWork unitOfWork)
        {
            _testCaseRepository = testCaseRepository;
            _logger = logger;
            _unitOfWork = unitOfWork;
        }
        public async Task<TestCase> CreateTestCaseAsync(string visibility, string inputText, string expectedOutputText, int weight, Guid problemId, CancellationToken ct)
        {
            var existing = await _testCaseRepository.GetByInputOutputAndProblemAsync(inputText, expectedOutputText, problemId, ct);
            if (existing != null)
            {
                _logger.LogWarning($"Test case with input {inputText}, output {expectedOutputText} and problem {problemId} exists");
                throw new DuplicateEntityException<TestCase>("input, output and problem ID", $"{inputText}, {expectedOutputText}, {problemId}");
            }
            var testCase = new TestCase
            {
                TestId = Guid.NewGuid(),
                Visibility = visibility,
                ProblemId = problemId,
                InputText = inputText,
                ExpectedOutputText = expectedOutputText,
                Weight = weight
            };
            _logger.LogInformation("Test case created successfully");

            await _testCaseRepository.AddAsync(testCase, ct);
            _logger.LogInformation("Test case added successfully");

            await _unitOfWork.SaveChangesAsync(ct);
            _logger.LogInformation("Test case saved to DB successfully");

            return testCase;
        }
        public async Task<TestCase?> FindTestCaseByIdAsync(Guid testId, CancellationToken ct)
        {
            return await _testCaseRepository.GetByIdAsync(testId, ct);
        }
        public async Task<IEnumerable<TestCase>> ReadAllTestCasesAsync(CancellationToken ct)
        {
            return await _testCaseRepository.GetAsync(ct);
        }
        public async Task<TestCase?> UpdateTestCaseAsync(TestCase newTestCase, CancellationToken ct)
        {
            throw new NotImplementedException();
        }
        public async Task DeleteTestCaseAsync(TestCase testCaseToDelete, CancellationToken ct)
        {

        }
    }
}
