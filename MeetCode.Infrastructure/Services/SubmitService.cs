using MeetCode.Application.DTOs.Other;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Services
{
    public class SubmitService : ISubmitService
    {
        private readonly IDockerValidator _dockerValidator;
        private readonly ILogger<SubmitService> _logger;
        public SubmitService(IDockerValidator dockerValidator,
            ILogger<SubmitService> logger)
        {
            _dockerValidator = dockerValidator;
            _logger = logger;
        }

        public async Task<TestResult> RunCodeAsync(string code, Language language, ProblemTemplate problemTemplate, TestCase testCase, CancellationToken ct)
        {
            var result = await _dockerValidator.RunCodeAsync(code, language, problemTemplate, testCase, ct);

            if (result == null)
            {
                _logger.LogWarning("Something went wrong while try to run code in the container, result is null");
                throw new Exception("Something went wrong while try to run code in the container, result is null");
            }

            return result;
        }

        public bool IsSubmissionAccepted(List<TestResult> testResults)
        {
            foreach (var testResult in testResults)
            {
                if (testResult.IsSuccessful == false)
                {
                    return false;
                }
            }

            return true;
        }

    }
}
