using MediatR;
using Ardalis.Result;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.DTOs.Response.Auth;

namespace MeetCode.Application.Commands.CommandEntities.Auth;

public sealed record RegisterUserCommand(string Email, string DisplayName, string Password)
    : IRequest<Result<RegisterResponse>>;
