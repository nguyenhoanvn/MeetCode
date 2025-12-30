using Ardalis.Result;
using MediatR;
using MeetCode.Application.DTOs.Response.Submit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryEntities.Submission
{
    public sealed record SubmissionAllUserQuery(Guid UserId, Guid ProblemId) : IRequest<Result<SubmissionAllResponse>>;
}
