using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Request.Submit
{
    public sealed record RunCodeRequest(
        string Code,
        Guid LanguageId,
        Guid ProblemId,
        List<Guid> TestCaseIds
        );
}
