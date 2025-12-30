using Ardalis.Result;
using MediatR;
using MeetCode.Application.DTOs.Response.Problem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryEntities.Problem
{
    public sealed record ProblemReadBySlugQueryUser(string Slug) : IRequest<Result<ProblemResponse>>;
}
