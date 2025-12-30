using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.DTOs.Response.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandEntities.Auth
{
    public sealed record RefreshTokenCommand(string PlainRefreshToken) : IRequest<Result<RefreshTokenResponse>>;
}
