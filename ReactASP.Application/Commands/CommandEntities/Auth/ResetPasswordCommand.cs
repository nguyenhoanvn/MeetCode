using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using ReactASP.Application.Commands.CommandResults.Auth;

namespace ReactASP.Application.Commands.CommandEntities.Auth
{
    public sealed record ResetPasswordCommand(
        string Code,
        string Email,
        string NewPassword
        ) : IRequest<Result<ResetPasswordResult>>;
}
