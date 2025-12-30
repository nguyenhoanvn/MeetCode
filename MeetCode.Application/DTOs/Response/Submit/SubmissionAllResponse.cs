using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Response.Submit
{
    public sealed record SubmissionAllResponse(List<SubmissionResponse> Submissions);
}
