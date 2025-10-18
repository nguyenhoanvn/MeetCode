using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Auth;

namespace MeetCode.Application.Commands.CommandValidators.Auth
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(x => x.Code)
                .NotEmpty().WithMessage("OTP is required.")
                .Length(6).WithMessage("Invalid OTP.")
                .Matches("^[0-9]$+").WithMessage("OTP must contains numeric characters only");
            RuleFor(x => x.NewPassword)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must have more than 8 characters.")
                .Matches("[a-zA-Z]").WithMessage("Password must contain at least one alphabetical letter.")
                .Matches("[0-9]").WithMessage("Password must contain at least one number.");
        }
    }
}
