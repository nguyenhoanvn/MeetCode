using System;

namespace ReactASP.Application.Commands.RegisterUser;

public sealed record RegisterUserResult(Guid userId, string email, string displayName, string role);
