using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.DTOs.Response.Auth;

namespace MeetCode.Application.Commands.CommandEntities.Auth
{
    public sealed record ResetPasswordCommand(
        string Email,
        string NewPassword
        ) : IRequest<Result<ResetPasswordResponse>>;
}
