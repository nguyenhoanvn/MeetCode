using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReactASP.Application.Commands.CommandEntities.Auth;
using ReactASP.Application.Commands.CommandResults.Auth;
using ReactASP.Application.Interfaces;
using ReactASP.Application.Interfaces.Services;

namespace ReactASP.Application.Commands.CommandHandlers.Auth
{
    public sealed class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordResult>>
    {
        private readonly ICacheService _cacheService;
        private readonly IUserService _userService;
        private readonly IUnitOfWork _unitOfWork;
        public ResetPasswordCommandHandler(ICacheService cacheService,
            IUserService userService,
            IUnitOfWork unitOfWork)
        {
            _cacheService = cacheService;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<ResetPasswordResult>> Handle(ResetPasswordCommand request, CancellationToken ct)
        {
            var code = _cacheService.GetValueAsync("ResetPasswordOTP");

            if (code.Result == null)
            {
                return Result.Invalid(new ValidationError
                {
                    Identifier = nameof(code),
                    ErrorMessage = "Code not in cache"
                });
            }

            int requestCode = int.Parse(request.Code);
            int cacheCode = int.Parse(code.Result);

            if (cacheCode != requestCode)
            {
                var resultUnmatch = new ResetPasswordResult(Message: "Incorrect OTP");
                return Result.Success(resultUnmatch);
            }

            var user = await _userService.FindUserByEmailAsync(request.Email, ct);
            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            if (newPasswordHash.Equals(user.PasswordHash))
            {
                var resultPasswordSame = new ResetPasswordResult(Message: "New password cannot same with old password");
                return Result.Success(resultPasswordSame);
            }
            user.PasswordHash = newPasswordHash;
            await _cacheService.RemoveValueAsync("ResetPasswordOTP");
            await _unitOfWork.SaveChangesAsync(ct);

            var result = new ResetPasswordResult(Message: "Password changed successfully");
            return Result.Success();
        }
    }
}
