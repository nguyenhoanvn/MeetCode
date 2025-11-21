namespace MeetCode.Application.DTOs.Request.Auth
{
    public sealed record ResetPasswordRequest(
        string Email,
        string NewPassword
        );
}
