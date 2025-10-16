using AutoMapper;
using ReactASP.Application.Commands.CommandEntities.Auth;
using ReactASP.Application.Commands.CommandResults.Auth;
using ReactASP.Application.Queries.QueryEntities.Auth;
using ReactASP.Application.Queries.QueryResults.Auth;
using ReactASP.Server.DTOs.Request.Auth;
using ReactASP.Server.DTOs.Response.Auth;

namespace ReactASP.Server.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            // Login
            CreateMap<LoginRequest, LoginUserQuery>();
            CreateMap<LoginUserQueryResult, LoginResponse>();

            // Register
            CreateMap<RegisterRequest, RegisterUserCommand>();
            CreateMap<RegisterUserResult, RegisterResponse>();

            // RefreshToken
            CreateMap<RefreshTokenRequest, RefreshTokenCommand>();
            CreateMap<RefreshTokenResult, RefreshTokenResponse>();

            // ForgotPassword
            CreateMap<ForgotPasswordRequest, ForgotPasswordQuery>();
            CreateMap<ForgotPasswordQueryResult, ForgotPasswordResponse>();

            // ResetPassword
            CreateMap<ResetPasswordRequest, ResetPasswordCommand>()
                .ForCtorParam("Email", opt => opt.MapFrom((src, ctx) => ctx.Items["Email"]));
            CreateMap<ResetPasswordResult, ResetPasswordResponse>();
        }
    }
}
