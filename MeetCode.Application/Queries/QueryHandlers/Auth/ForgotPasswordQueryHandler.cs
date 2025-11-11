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

namespace MeetCode.Application.Queries.QueryHandlers.Auth
{
    public class ForgotPasswordQueryHandler : IRequestHandler<ForgotPasswordQuery, Result<ForgotPasswordQueryResult>>
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

        public async Task<Result<ForgotPasswordQueryResult>> Handle(ForgotPasswordQuery request, CancellationToken ct)
        {
            var user = await _userService.FindUserByEmailAsync(request.Email, ct);
            if (user == null)
            {
                _logger.LogInformation("No user with such email stored in database");
                var resultAlt = new ForgotPasswordQueryResult(
                    CurrentUser: null,
                    Message: "No account got associate with this email"
                    );
                return Result.Success(resultAlt);
            }
            _logger.LogInformation($"User with email {request.Email} found");

            var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();

            // Store cache to find OTP by Email later
            var cacheOTPKey = $"auth:resetpassword:email:{user.Email}";
            await _cacheService.SetValueAsync(cacheOTPKey, code, TimeSpan.FromMinutes(5));

            await _emailService.SendEmailAsync(
                to: request.Email,
                subject: "Reetlank reset password verification code",
                body: $"<p>Your reset password verification code is {code}</p>"
                );

            var result = new ForgotPasswordQueryResult(
                CurrentUser: user,
                Message: "A verification code has been sent to your email"
                );
            return Result.Success(result);

        }
    }
}
