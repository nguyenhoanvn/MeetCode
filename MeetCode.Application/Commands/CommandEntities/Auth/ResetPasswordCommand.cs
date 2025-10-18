using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Auth;

namespace MeetCode.Application.Commands.CommandEntities.Auth
{
    public sealed record ResetPasswordCommand(
        string Code,
        string NewPassword
        ) : IRequest<Result<ResetPasswordResult>>;
}
