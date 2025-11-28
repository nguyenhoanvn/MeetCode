using MeetCode.Application.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandResults.Submit
{
    public sealed record RunCodeCommandResult(
        List<TestResult> TestResults
        );
}
