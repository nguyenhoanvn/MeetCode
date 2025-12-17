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
using MeetCode.Application.DTOs.Response.Auth;
using AutoMapper;

namespace MeetCode.Application.Commands.CommandHandlers.Auth;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterResponse>>
{
    private readonly ILogger<RegisterUserCommandHandler> _logger;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;
    public RegisterUserCommandHandler(
        ILogger<RegisterUserCommandHandler> logger,
        IUserService userService,
        IMapper mapper)
    {
        _logger = logger;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<Result<RegisterResponse>> Handle(RegisterUserCommand request, CancellationToken ct)
    {
        _logger.LogInformation("Register handler started for user {Email}", request.Email);

        var resultUser = await _userService.CreateUserAsync(request.Email, request.DisplayName, request.Password, ct);

        if (resultUser.IsError())
        {
            _logger.LogWarning("User creation failed for {Email}. Errors: {Errors}",
                request.Email,
                string.Join("\n", resultUser.Errors));
            return Result.Error(string.Join("\n", resultUser.Errors));
        }

        if (!resultUser.IsSuccess)
        {
            _logger.LogWarning("Validation failed for {Email}. Errors: {ValidationErrors}",
                request.Email,
                string.Join("\n", resultUser.ValidationErrors.Select(e => $"{e.Identifier}: {e.ErrorMessage}")));
            return Result.Invalid(resultUser.ValidationErrors);
        }

        var user = resultUser.Value;
        _logger.LogInformation($"An user has been created successfully with Id: {user.UserId}");

        return Result.Success(new RegisterResponse());
    }
}