using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;

namespace ReactASP.Application.Commands.RefreshToken
{
    public sealed record RefreshTokenCommand(string refreshToken) : IRequest<Result<RefreshTokenResult>>;
}
