using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Repositories
{
    public interface ITestCaseRepository : IRepository<TestCase>
    {
        Task<bool> IsTestCaseExistsAsync(string inputText, string outputText, Guid problemId, CancellationToken ct);
    }
}
