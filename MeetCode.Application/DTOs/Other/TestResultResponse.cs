using MeetCode.Application.DTOs.Response.TestCase;
using MeetCode.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Other
{
    public sealed record TestResultResponse(
        TestCaseResponse TestCase, 
        string Result, 
        bool IsSuccessful, 
        long ExecTimeMs
        );
}
