using Ardalis.Result;
using MediatR;
using MeetCode.Application.DTOs.Response.Auth;
using MeetCode.Application.Queries.QueryResults.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryEntities.Auth
{
    public sealed record VerifyResetPasswordOTPQuery(
        string Email,
        string Code
        ) : IRequest<Result<VerifyResetPasswordOTPResponse>>;
}
