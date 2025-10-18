using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace MeetCode.Application.Commands.CommandHandlers.Auth
{
    public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordResult>>
    {
        private readonly ICacheService _cacheService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthService _authService;
        public ResetPasswordCommandHandler(ICacheService cacheService,
            IUserService userService,
            IUnitOfWork unitOfWork,
            IAuthService authService)
        {
            _cacheService = cacheService;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<Result<ResetPasswordResult>> Handle(ResetPasswordCommand request, CancellationToken ct)
        {
            var email = await _authService.GetEmailFromOtpAsync(request.Code);
            var cacheKey = $"auth:resetpassword:email:{email}";
            var code = await _cacheService.GetValueAsync<string>(cacheKey);

            if (code == null)
            {
                return Result.Invalid(new ValidationError
                {
                    Identifier = nameof(code),
                    ErrorMessage = "Code not in cache"
                });
            }

            int requestCode = int.Parse(request.Code);
            int cacheCode = int.Parse(code);

            if (cacheCode != requestCode)
            {
                var resultUnmatch = new ResetPasswordResult(Message: "Incorrect OTP");
                return Result.Success(resultUnmatch);
            }

            var user = await _userService.FindUserByEmailAsync(email, ct);
            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            if (newPasswordHash.Equals(user.PasswordHash))
            {
                var resultPasswordSame = new ResetPasswordResult(Message: "New password cannot same with old password");
                return Result.Success(resultPasswordSame);
            }
            user.PasswordHash = newPasswordHash;
            await _cacheService.RemoveValueAsync($"auth:resetpassword:email:{email}");
            await _cacheService.RemoveValueAsync($"auth:resetpassword:otp:{code}");
            await _unitOfWork.SaveChangesAsync(ct);

            var result = new ResetPasswordResult(Message: "Password changed successfully");
            return Result.Success(result);
        }
    }
}
