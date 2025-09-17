using System;

namespace ReactASP.Application.Commands.RegisterUser;

public sealed record RegisterUserResult(Guid UserId, string Email, string DisplayName, string Role);
