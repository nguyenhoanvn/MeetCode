using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MeetCode.Application.Commands.CommandResults.Auth;

namespace MeetCode.Application.Commands.CommandEntities.Auth
{
    public sealed record RefreshTokenCommand(string PlainRefreshToken) : IRequest<Result<RefreshTokenResult>>;
}
