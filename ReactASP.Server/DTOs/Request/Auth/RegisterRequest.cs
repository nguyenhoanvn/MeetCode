namespace ReactASP.Server.DTOs.Request.Auth;

public sealed record RegisterRequest(
        string Email,
        string DisplayName,
        string Password
    );