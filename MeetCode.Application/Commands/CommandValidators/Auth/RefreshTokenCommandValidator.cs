using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Auth;

namespace MeetCode.Application.Commands.CommandValidators.Auth
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator()
        {
            RuleFor(x => x.PlainRefreshToken)
                .NotEmpty().WithMessage("Refresh token is required.");
        }
    }
}
