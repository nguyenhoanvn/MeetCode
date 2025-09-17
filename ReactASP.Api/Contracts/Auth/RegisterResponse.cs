using System;

namespace ReactASP.Api.Contracts.Auth;

public sealed class RegisterResponse
{
    public Guid UserId { get; init; }
    public string Email { get; init; } = default!;
    public string DisplayName { get; init; } = default!;
    public string Role { get; init; } = default!;
}