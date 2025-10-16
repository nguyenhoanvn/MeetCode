using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ReactASP.Application.Queries.QueryEntities.Auth;

namespace ReactASP.Application.Queries.QueryValidators.Auth
{
    public sealed class ForgotPasswordQueryValidator : AbstractValidator<ForgotPasswordQuery>
    {
        public ForgotPasswordQueryValidator()
        {
            RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .EmailAddress().WithMessage("Input must in email address format.");
        }
    }
}
