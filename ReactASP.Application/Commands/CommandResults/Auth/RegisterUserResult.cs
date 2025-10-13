using System;

namespace ReactASP.Application.Commands.CommandResults.Auth;

public sealed record RegisterUserResult(Guid UserId, string Email, string DisplayName, string Role);
