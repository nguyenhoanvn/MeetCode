using MediatR;
using Ardalis.Result;

namespace ReactASP.Application.Commands.RegisterUser;

public sealed record RegisterUserCommand(string Email, string DisplayName, string Password)
    : IRequest<Result<RegisterUserResult>>;
