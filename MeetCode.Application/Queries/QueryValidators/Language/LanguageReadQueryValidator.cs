using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Queries.QueryEntities.Language;

namespace MeetCode.Application.Queries.QueryValidators.Language
{
    public class LanguageReadQueryValidator : AbstractValidator<LanguageReadQuery>
    {
        public LanguageReadQueryValidator()
        {
            RuleFor(x => x.LangId)
                .NotEmpty().WithMessage("Language ID is required.")
                .Must(l => l.GetType() == typeof(Guid)).WithMessage("Language ID is invalid");
        }
    }
}
