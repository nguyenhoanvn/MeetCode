using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandValidators.Language
{
    public class LanguageStatusToggleCommandValidator : AbstractValidator<LanguageStatusToggleCommand>
    {
        public LanguageStatusToggleCommandValidator()
        {
            RuleFor(x => x.LangId)
                .NotEmpty().WithMessage("Language Id is required.")
                .Must(x => x.GetType() == typeof(Guid)).WithMessage("Language Id type mismatch.");
        }
    }
}
