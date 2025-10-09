using AutoMapper;
using ReactASP.Application.Commands.LoginUser;
using ReactASP.Application.Commands.RefreshToken;
using ReactASP.Application.Commands.RegisterUser;
using ReactASP.Application.DTOs.LoginUser;
using ReactASP.Application.DTOs.RefreshToken;
using ReactASP.Application.DTOs.RegisterUser;
using ReactASP.Server.DTOs.RefreshToken;

namespace ReactASP.Server.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            // Login
            CreateMap<LoginRequest, LoginUserCommand>();
            CreateMap<LoginUserResult, LoginResponse>();

            // Register
            CreateMap<RegisterRequest, RegisterUserCommand>();
            CreateMap<RegisterUserResult, RegisterResponse>();

            // RefreshToken
            CreateMap<RefreshTokenRequest, RefreshTokenCommand>();
            CreateMap<RefreshTokenResult, RefreshTokenResponse>();
        }
    }
}
