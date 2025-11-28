using MeetCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Interfaces.Services
{
    public class TestResult
    {
        TestCase TestCase { get; set; } = default!;
        string Result { get; set; } = default!;
        bool IsSuccessful { get; set; } = default!;
    }
    public interface ISubmitService
    {
        Task<TestResult> RunCodeAsync(string code, Language language, Problem problem, List<TestCase> testCaseList, CancellationToken ct);
    }
}
