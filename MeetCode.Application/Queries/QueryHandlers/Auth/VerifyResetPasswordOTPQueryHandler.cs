using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Auth;
using MeetCode.Application.Queries.QueryResults.Auth;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Queries.QueryHandlers.Auth
{
    public class VerifyResetPasswordOTPQueryHandler : IRequestHandler<VerifyResetPasswordOTPQuery, Result<VerifyResetPasswordOTPQueryResult>>
    {
        private readonly ILogger<VerifyResetPasswordOTPQueryHandler> _logger;
        private readonly ICacheService _cacheService;
        private readonly IAuthService _authService;
        public VerifyResetPasswordOTPQueryHandler(
            ICacheService cacheService,
            ILogger<VerifyResetPasswordOTPQueryHandler> logger,
            IAuthService authService)
        {
            _cacheService = cacheService;
            _logger = logger;
            _authService = authService;
        }

        public async Task<Result<VerifyResetPasswordOTPQueryResult>> Handle(VerifyResetPasswordOTPQuery request, CancellationToken ct)
        {        
            var cacheKey = $"auth:resetpassword:email:{request.Email}";
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
                var resultUnmatch = new VerifyResetPasswordOTPQueryResult(false);
                return Result.Success(resultUnmatch);
            }

            var result = new VerifyResetPasswordOTPQueryResult(true);
            return Result.Success(result);
        }
    }
}
