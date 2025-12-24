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
                .WithMessage("Method name cannot start with a digit")
                .Must(name => name.All(c => char.IsLetterOrDigit(c) || c == '_'))
                .WithMessage("Method name can only contain letters, digits or underscores");

            RuleFor(x => x.ReturnType)
                .NotEmpty().WithMessage("Method return type cannot be empty")
                .Must(IsValidType)
                .WithMessage("Method return type must be a valid type (can contain letters, digits, brackets, spaces, and common type characters)");

            RuleFor(x => x.Parameters)
                .NotNull().WithMessage("Parameters cannot be null")
                .NotEmpty().WithMessage("Parameters cannot be empty")
                .Must(parameters => parameters.All(p => !string.IsNullOrWhiteSpace(p)))
                .WithMessage("Parameters cannot contain null or empty values")
                .Must(parameters => parameters.All(IsValidParameter))
                .WithMessage("Each parameter must be in the format 'type name' (e.g., 'int[] nums', 'string text')");

            RuleFor(x => x.ProblemId)
                .NotEmpty().WithMessage("Problem Id cannot be empty");

            RuleFor(x => x.LangId)
                .NotEmpty().WithMessage("Language Id cannot be empty");
        }

        private bool IsValidType(string type)
        {
            if (string.IsNullOrWhiteSpace(type))
                return false;

            // Allow letters, digits, brackets [], spaces, and common generic characters like < > ,
            return type.All(c => char.IsLetterOrDigit(c) || c == '[' || c == ']' || c == ' ' || c == '<' || c == '>' || c == ',');
        }

        private bool IsValidParameter(string parameter)
        {
            if (string.IsNullOrWhiteSpace(parameter))
                return false;

            var parts = parameter.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Must have at least 2 parts: type and name (e.g., "int x" or "int[] nums")
            if (parts.Length < 2)
                return false;

            // Last part is the parameter name
            var paramName = parts[^1];

            // Parameter name validation (same rules as method name)
            if (string.IsNullOrEmpty(paramName) || char.IsDigit(paramName[0]))
                return false;

            if (!paramName.All(c => char.IsLetterOrDigit(c) || c == '_'))
                return false;

            // Type is everything except the last part
            var type = string.Join(" ", parts.Take(parts.Length - 1));

            return IsValidType(type);
        }
    }
}
