using System;

namespace MeetCode.Application.DTOs.Response.Auth;

public sealed record RegisterResponse(
    Guid UserId,
    string Email,
    string DisplayName,
    string Role
    );