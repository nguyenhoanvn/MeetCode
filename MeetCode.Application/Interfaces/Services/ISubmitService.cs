using MeetCode.Application.DTOs.Other;
using MeetCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Interfaces.Services
{
    public interface ISubmitService
    {
        Task<TestResult> RunCodeAsync(string code, Language language, ProblemTemplate problem, TestCase testCase, CancellationToken ct);
        bool IsSubmissionAccepted(List<TestResult> testResults);
    }
}
