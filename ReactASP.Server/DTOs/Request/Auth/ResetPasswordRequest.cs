namespace ReactASP.Server.DTOs.Request.Auth
{
    public sealed record ResetPasswordRequest(
        string Code,
        string NewPassword
        );
}
