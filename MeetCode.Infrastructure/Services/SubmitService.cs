using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Infrastructure.Services
{
    public class SubmitService : ISubmitService
    {
        public SubmitService()
        {

        }

        public async Task<TestResult> RunCodeAsync(string code, Language language, Problem problem, TestCase testCase, CancellationToken ct)
        {
            throw new NotImplementedException();
        }

    }
}
