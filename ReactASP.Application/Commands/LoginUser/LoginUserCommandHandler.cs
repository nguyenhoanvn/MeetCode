using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Application.Interfaces;
using Ardalis.Result;


namespace ReactASP.Application.Commands.LoginUser
{
    public sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<LoginUserResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public LoginUserCommandHandler(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<Result<LoginUserResult>> Handle(LoginUserCommand request, CancellationToken ct)
        {
            var email = request.Email.Trim().ToLowerInvariant();

            var user = await _userRepository.GetUserByEmailAsync(email, ct);
            if (user is null) throw new UnauthorizedAccessException("Invalid email or password.");

            var ok = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!ok) throw new UnauthorizedAccessException("Invalid email or password.");

            string accessToken = _tokenService.GenerateJwtToken(user.UserId, user.Email, user.Role);
            string refreshToken = _tokenService.GenerateRefreshToken();
            return Result.Success(new LoginUserResult(accessToken, refreshToken, user.DisplayName, user.Role));
        }
    }
}
