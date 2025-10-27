using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Services
{
    public interface ITestCaseService
    {
        Task<TestCase> CreateTestCaseAsync(string visibility, string inputText, string expectedOutputText, int weight, Guid problemId, CancellationToken ct);
        Task<TestCase?> FindTestCaseByIdAsync(Guid testId, CancellationToken ct);
        Task<IEnumerable<TestCase>> ReadAllTestCasesAsync(CancellationToken ct);
        Task<TestCase?> UpdateTestCaseAsync(TestCase newTestCase, CancellationToken ct);
        Task DeleteTestCaseAsync(TestCase testCaseToDelete, CancellationToken ct);
    }
}
