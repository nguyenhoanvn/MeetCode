namespace MeetCode.Server.DTOs.Request.Auth
{
    public sealed record VerifyResetPasswordOTPRequest(
        string Email,
        string Code
        );
}
