using System.Security.Cryptography;
using System.Text;
using MediatR;
using MeetCode.Domain.Entities;
using Ardalis.Result;
using Microsoft.Extensions.Logging;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Domain.Enums;
using MeetCode.Application.Interfaces.Services;

namespace MeetCode.Application.Commands.CommandHandlers.Auth;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResult>>
{
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    private readonly IUserService _userService;
    public RegisterUserCommandHandler(
        ILogger<RegisterUserCommandHandler> logger,
        IUserService userService)
    {
        _logger = logger;
        _userService = userService;
    }

    public async Task<Result<RegisterUserResult>> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        _logger.LogInformation($"Register handler started for user {request.Email}");

        var user = await _userService.CreateUserAsync(request.Email, request.DisplayName, request.Password, ct);

        _logger.LogInformation($"An user has been created successfully with Id: {user.UserId}");

        var result = new RegisterUserResult(user.UserId, user.Email, user.DisplayName, user.Role);

        return Result.Success(result);

    }
}