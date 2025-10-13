using AutoMapper;
using ReactASP.Application.Commands.CommandEntities.Auth;
using ReactASP.Application.Commands.CommandResults.Auth;
using ReactASP.Application.DTOs.LoginUser;
using ReactASP.Server.DTOs.Request;
using ReactASP.Server.DTOs.Response;

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

            // ForgotPassword
            CreateMap<ForgotPasswordRequest, ForgotPasswordCommand>();
            CreateMap<ForgotPasswordResult, ForgotPasswordResponse>();

            // ResetPassword
            CreateMap<ResetPasswordRequest, ResetPasswordCommand>();
            CreateMap<ResetPasswordResult, ResetPasswordResponse>();
        }
    }
}
