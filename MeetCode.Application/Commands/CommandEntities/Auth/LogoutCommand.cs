using MediatR;
using MeetCode.Application.Commands.CommandResults.Auth;
using Ardalis.Result;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandEntities.Auth
{
    public sealed record LogoutCommand(Guid UserId) : IRequest<Result<LogoutCommandResult>>;
}
