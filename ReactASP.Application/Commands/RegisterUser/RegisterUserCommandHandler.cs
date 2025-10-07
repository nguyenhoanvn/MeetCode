using System.Security.Cryptography;
using System.Text;
using MediatR;
using ReactASP.Domain.Enums;
using ReactASP.Domain.Entities;
using ReactASP.Application.Interfaces;
using Ardalis.Result;
using Microsoft.Extensions.Logging;

namespace ReactASP.Application.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<RegisterUserResult>> {
    
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterUserCommandHandler> _logger;


    public RegisterUserCommandHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<RegisterUserCommandHandler> logger) {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result<RegisterUserResult>> Handle(RegisterUserCommand request, CancellationToken ct) {

        try
        {
            var email = request.Email.Trim().ToLowerInvariant();

            if (await _userRepository.EmailExistsAsync(email, ct))
            {
                _logger.LogWarning("Email already exists");
                return Result.Invalid(new ValidationError
                {
                    Identifier = nameof(email),
                    ErrorMessage = "Email already exists"
                });
            }

            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                UserId = Guid.NewGuid(),
                Email = email,
                DisplayName = request.DisplayName.Trim(),
                Role = UserRole.User,
                PasswordHash = hashedPassword,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            await _userRepository.AddAsync(user, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            _logger.LogInformation($"An user has been created successfully with Id: {user.UserId}");

            return Result.Success(new RegisterUserResult(user.UserId, user.Email, user.DisplayName, user.Role));
        } catch (Exception ex)
        {
            _logger.LogError($"An exception occured: {ex.Message}");
            return Result.Error("An unexpected error occured while register a new user");
        }
        
    }
}