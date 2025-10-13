using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using RabbitMQ.Client;
using Microsoft.Extensions.Logging;
using ReactASP.Application.Commands.CommandEntities.Auth;
using ReactASP.Application.Commands.CommandResults.Auth;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Application.Interfaces.Repositories;
using System.Security.Cryptography;

namespace ReactASP.Application.Commands.CommandHandlers.Auth
{
    public sealed class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, Result<ForgotPasswordResult>>
    {

        private readonly IUserService _userService;
        private readonly IEmailService _emailService;
        private readonly ILogger<ForgotPasswordCommand> _logger;
        private readonly ICacheService _cacheService;

        public ForgotPasswordCommandHandler(
            IUserService userService,
            IEmailService emailService,
            ILogger<ForgotPasswordCommand> logger,
            ICacheService cacheService)
        {
            _userService = userService;
            _emailService = emailService;
            _logger = logger;
            _cacheService = cacheService;
        }

        public async Task<Result<ForgotPasswordResult>> Handle(ForgotPasswordCommand request, CancellationToken ct)
        {
            try
            {
                _logger.LogInformation("Forgot password hanlder started");
                var user = await _userService.FindUserByEmailAsync(request.Email, ct);
                if (user == null)
                {
                    _logger.LogInformation("No user with such email stored in database");
                    var resultAlt = new ForgotPasswordResult(
                        CurrentUser: null,
                        Message: "No account got associate with this email"
                        );
                    return Result.Success(resultAlt);
                }
                _logger.LogInformation($"User with email {request.Email} found");

                var code = RandomNumberGenerator.GetInt32(100000, 999999).ToString();
                await _cacheService.SetValueAsync("Reset_Password_OTP", code, TimeSpan.FromMinutes(1));
                await _emailService.SendEmailAsync(
                    to: request.Email,
                    subject: "Reetlank reset password verification code",
                    body: $"<p>Your reset password verification code is {code}</p>"
                    );

                var result = new ForgotPasswordResult(
                    CurrentUser: user,
                    Message: "A verification code has been sent to your email"
                    );
                return Result.Success(result);
            } catch (Exception ex)
            {
                _logger.LogError($"An exception occured while trying to check forgot password user: {ex.Message}");
                return Result.Error("An exception occured while forgot password");
            }
            
        }
    }
}
