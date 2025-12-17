using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.DTOs.Response.Auth;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.Auth
{
    public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordResponse>>
    {
        private readonly ICacheService _cacheService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        private readonly ILogger<ResetPasswordCommandHandler> _logger;
        public ResetPasswordCommandHandler(ICacheService cacheService,
            IUserService userService,
            IUnitOfWork unitOfWork,
            IAuthService authService,
            ILogger<ResetPasswordCommandHandler> logger)
        {
            _cacheService = cacheService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _authService = authService;
            _logger = logger;
        }

        public async Task<Result<ResetPasswordResponse>> Handle(ResetPasswordCommand request, CancellationToken ct)
        {

            var userResult = await _userService.FindUserByEmailAsync(request.Email, ct);
            if (!userResult.IsSuccess)
            {
                _logger.LogWarning("Cannot find user with email {Email}", request.Email);
                return Result.Invalid(userResult.ValidationErrors);
            }

            var user = userResult.Value;

            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            if (newPasswordHash.Equals(user.PasswordHash))
            {
                _logger.LogWarning("New password cannot the same with old password");
                return Result.Invalid(new ValidationError(nameof(request.NewPassword), "New password cannot the same with old password"));
            }
            user.PasswordHash = newPasswordHash;

            await _cacheService.RemoveValueAsync($"auth:resetpassword:email:{request.Email}");
            
            await _unitOfWork.SaveChangesAsync(ct);

            return Result.Success(new ResetPasswordResponse());
        }
    }
}
