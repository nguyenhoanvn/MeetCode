using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Request.Problem
{
    public sealed record ProblemListRequest(
        int? PageNumber, 
        int? PageSize
        );
}
