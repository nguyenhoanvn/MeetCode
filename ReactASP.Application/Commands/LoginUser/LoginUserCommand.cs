using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;

namespace ReactASP.Application.Commands.LoginUser
{
    public sealed record LoginUserCommand(string Email, string Password) : IRequest<Result<LoginUserResult>>;
}
