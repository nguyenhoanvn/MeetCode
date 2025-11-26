using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.ProblemTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandValidators.ProblemTemplate
{
    public class ProblemTemplateAddCommandValidator : AbstractValidator<ProblemTemplateAddCommand>
    {
        public ProblemTemplateAddCommandValidator()
        {
            RuleFor(x => x.MethodName)
                .NotEmpty().WithMessage("Method name cannot be empty")
                .Must(name => !string.IsNullOrEmpty(name) && !char.IsDigit(name[0]))
                .WithMessage("Method name cannot starts with a digit")
                .Must(name => name.All(c => char.IsLetterOrDigit(c) || c == '_'))
                .WithMessage("Method name can only contains letters, digits or underscores");
            RuleFor(x => x.ReturnType)
                .NotEmpty().WithMessage("Method return type cannot be empty")
                .Must(type => type.All(c => char.IsLetterOrDigit(c)))
                .WithMessage("Method return type can only contains letters and digits");
            RuleFor(x => x.ProblemId)
                .NotEmpty().WithMessage("Problem Id cannot be empty")
                .Must(id => id.GetType() == typeof(Guid)).WithMessage("Problem Id must be a Guid");
            RuleFor(x => x.LangId)
                .NotEmpty().WithMessage("Language Id cannot be empty")
                .Must(id => id.GetType() == typeof(Guid)).WithMessage("Language Id must be a Guid");
        }
    }
}
