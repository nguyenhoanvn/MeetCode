namespace MeetCode.Application.DTOs.Response.Auth
{
    public sealed record LoginResponse(
        string AccessToken,
        string RefreshToken,
        string DisplayName
        );
}
