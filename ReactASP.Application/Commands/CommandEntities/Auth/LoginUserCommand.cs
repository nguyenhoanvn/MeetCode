using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using ReactASP.Application.Commands.CommandResults.Auth;

namespace ReactASP.Application.Commands.CommandEntities.Auth
{
    public sealed record LoginUserCommand(string Email, string Password) : IRequest<Result<LoginUserResult>>;
}
