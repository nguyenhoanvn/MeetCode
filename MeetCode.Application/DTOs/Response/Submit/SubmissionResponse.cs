using MeetCode.Application.DTOs.Response.Language;
using MeetCode.Application.DTOs.Response.Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Response.Submit
{
    public sealed record SubmissionResponse(
        Guid SubmissionId,
        Guid UserId,
        LanguageResponse Language,
        ProblemResponse Problem,
        string Verdict,
        int ExecTimeMs
        );
}
