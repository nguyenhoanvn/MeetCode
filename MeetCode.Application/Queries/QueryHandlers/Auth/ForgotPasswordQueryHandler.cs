using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Auth;
using MeetCode.Application.Queries.QueryResults.Auth;
using Microsoft.Extensions.Logging;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.DTOs.Response.Auth;

namespace MeetCode.Application.Queries.QueryHandlers.Auth
{
    public class ForgotPasswordQueryHandler : IRequestHandler<ForgotPasswordQuery, Result<ForgotPasswordResponse>>
    {
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ILogger<ForgotPasswordQueryHandler> _logger;
        private readonly ICacheService _cacheService;
        public ForgotPasswordQueryHandler(
            IUserService userService,
            IEmailService emailService,
            ILogger<ForgotPasswordQueryHandler> logger,
            ICacheService cacheService)
        {
            _userService = userService;
            _emailService = emailService;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result<ForgotPasswordResponse>> Handle(ForgotPasswordQuery request, CancellationToken ct)
        {
            var userResult = await _userService.FindUserByEmailAsync(request.Email, ct);
            if (!userResult.IsSuccess)
            {
                _logger.LogInformation("No user with such email stored in database");
                return Result.Success(new ForgotPasswordResponse());
            }

            var user = userResult.Value;
            _logger.LogInformation($"User with email {request.Email} found");

            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            // Store cache to find OTP by Email later
            var cacheOTPKey = $"auth:resetpassword:email:{user.Email}";
            await _cacheService.SetValueAsync(cacheOTPKey, code, TimeSpan.FromMinutes(5));

            await _emailService.SendEmailAsync(
                to: request.Email,
                subject: "MeetCode reset password verification code",
                body: $"<p>Your reset password verification code is {code}</p>"
                );

            return Result.Success(new ForgotPasswordResponse());

        }
    }
}
