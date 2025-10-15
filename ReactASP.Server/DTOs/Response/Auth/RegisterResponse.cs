using System;

namespace ReactASP.Server.DTOs.Response.Auth;

public sealed record RegisterResponse(
    Guid UserId,
    string Email,
    string DisplayName,
    string Role
    );