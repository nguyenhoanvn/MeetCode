using MediatR;
using Ardalis.Result;
using ReactASP.Application.Commands.CommandResults.Auth;

namespace ReactASP.Application.Commands.CommandEntities.Auth;

public sealed record RegisterUserCommand(string Email, string DisplayName, string Password)
    : IRequest<Result<RegisterUserResult>>;
