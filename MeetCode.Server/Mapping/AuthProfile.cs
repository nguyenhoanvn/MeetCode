using AutoMapper;
using MeetCode.Application.Commands.CommandEntities.Auth;
using MeetCode.Application.Commands.CommandResults.Auth;
using MeetCode.Application.Queries.QueryEntities.Auth;
using MeetCode.Application.Queries.QueryResults.Auth;
using MeetCode.Server.DTOs.Request.Auth;
using MeetCode.Server.DTOs.Response.Auth;

namespace MeetCode.Server.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            // Login
            CreateMap<LoginRequest, LoginUserQuery>();
            CreateMap<LoginUserQueryResult, LoginResponse>()
                .ConstructUsing(src => new LoginResponse(
                    src.IsSuccessful,
                    src.Message
                    ));

            // Register
            CreateMap<RegisterRequest, RegisterUserCommand>();
            CreateMap<RegisterUserResult, RegisterResponse>();

            // RefreshToken
            CreateMap<RefreshTokenRequest, RefreshTokenCommand>();
            CreateMap<RefreshTokenResult, RefreshTokenResponse>()
                .ConstructUsing(src => new RefreshTokenResponse());

            // ForgotPassword
            CreateMap<ForgotPasswordRequest, ForgotPasswordQuery>();
            CreateMap<ForgotPasswordQueryResult, ForgotPasswordResponse>();

            // VerifyResetPasswordOTP
            CreateMap<VerifyResetPasswordOTPRequest, VerifyResetPasswordOTPQuery>();
            CreateMap<VerifyResetPasswordOTPQueryResult, VerifyResetPasswordOTPResponse>();

            // ResetPassword
            CreateMap<ResetPasswordRequest, ResetPasswordCommand>();
            CreateMap<ResetPasswordResult, ResetPasswordResponse>();
        }
    }
}
