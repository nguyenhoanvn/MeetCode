using MeetCode.Application.DTOs.Other;
using MeetCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Interfaces.Services
{
    public interface IDockerValidator
    {
        Task<bool> ImageExistsAsync(string image, CancellationToken ct);
        Task<TestResult?> RunCodeAsync(string code, Language language, ProblemTemplate problemTemplate, TestCase testCase, CancellationToken ct);
    }
}
