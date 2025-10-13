using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ReactASP.Application.Commands.CommandEntities.Auth;

namespace ReactASP.Application.Commands.CommandValidators.Auth
{
    public sealed class ForgotPasswordCommandValidator : AbstractValidator<ForgotPasswordCommand>
    {
        public ForgotPasswordCommandValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Input must in email address format.");
        }
    }
}
