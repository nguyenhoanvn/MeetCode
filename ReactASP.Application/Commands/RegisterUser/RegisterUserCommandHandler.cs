using System.Security.Cryptography;
using System.Text;
using MediatR;
using ReactASP.Domain.Enums;
using ReactASP.Domain.Entities;
using ReactASP.Application.Interfaces;
using Ardalis.Result;

namespace ReactASP.Application.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResult>> {
    private readonly IUserRepository _userRepository;

    public RegisterUserCommandHandler(IUserRepository userRepository) {
        _userRepository = userRepository;
    }

    public async Task<Result<RegisterUserResult>> Handle(RegisterUserCommand request, CancellationToken ct) {
        // Normalize email
        var email = request.Email.Trim().ToLowerInvariant();

        if (await _userRepository.EmailExistsAsync(email, ct)) {
            throw new InvalidOperationException("Email already exists");
        }

        string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

        var user = new User {
            UserId = Guid.NewGuid(),
            Email = email,
            DisplayName = request.DisplayName.Trim(),
            Role = UserRole.User,
            PasswordHash = hashedPassword,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
        };

        await _userRepository.AddAsync(user, ct);
        await _userRepository.SaveChangesAsync(ct);
        
        return Result.Success(new RegisterUserResult(user.UserId, user.Email, user.DisplayName, user.Role));
    }
}