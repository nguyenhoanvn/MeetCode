using MeetCode.Application.DTOs.Other;
using MeetCode.Application.DTOs.Response.TestCase;
using MeetCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Response.Submit
{
    public sealed record RunCodeResponse(
        Guid JobId,
        string Status,
        List<TestResultResponse> TestResults
        );
}
