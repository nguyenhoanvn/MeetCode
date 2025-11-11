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

            var user = await _userService.FindUserByEmailAsync(request.Email, ct);
            if (user == null)
            {
                return Result.Error("Cannot find the user in database");
            }
            var newPasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);
            if (newPasswordHash.Equals(user.PasswordHash))
            {
                var resultPasswordSame = new ResetPasswordResult(false);
                return Result.Success(resultPasswordSame);
            }
            user.PasswordHash = newPasswordHash;
            await _cacheService.RemoveValueAsync($"auth:resetpassword:email:{request.Email}");
            
            await _unitOfWork.SaveChangesAsync(ct);

            var result = new ResetPasswordResult(true);
            return Result.Success(result);
        }
    }
}
