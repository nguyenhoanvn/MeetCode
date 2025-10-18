using System;

namespace MeetCode.Server.DTOs.Response.Auth;

public sealed record RegisterResponse(
    Guid UserId,
    string Email,
    string DisplayName,
    string Role
    );