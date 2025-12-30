using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Request.Submit
{
    public sealed record RunCodeRequest(
        Guid UserId,
        string Code,
        string LanguageName,
        Guid ProblemId,
        List<Guid> TestCaseIds
        );
}
