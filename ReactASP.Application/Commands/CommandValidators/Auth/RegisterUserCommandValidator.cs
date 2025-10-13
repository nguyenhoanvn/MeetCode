using FluentValidation;
using ReactASP.Application.Commands.CommandEntities.Auth;

namespace ReactASP.Application.Commands.CommandValidators.Auth;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand> {
    public RegisterUserCommandValidator() {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Invalid email format.")
            .MaximumLength(255).WithMessage("Email must not greater than 255 characters.");

        RuleFor(x => x.DisplayName)
            .NotEmpty().WithMessage("Display name is required.")
            .MaximumLength(100).WithMessage("Display name must not greater than 100 characters.");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required.")
            .MinimumLength(8).WithMessage("Password must have more than 8 characters.")
            .Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
            .Matches("[0-9]").WithMessage("Password must contain at least one number.");
    }
}