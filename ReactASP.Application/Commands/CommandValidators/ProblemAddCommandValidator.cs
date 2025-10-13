using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ReactASP.Application.Commands.CommandEntities;

namespace ReactASP.Application.Commands.CommandValidators
{
    public sealed class ProblemAddCommandValidator : AbstractValidator<ProblemAddCommand>
    {
        public ProblemAddCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Problem title is required.")
                .MaximumLength(255).WithMessage("Problem title must less than 255 characters.")
                .Matches("^[a-zA-Z]").WithMessage("Problem must contain only alphabetical characters.");
            RuleFor(x => x.Difficulty)
                .NotEmpty().WithMessage("Problem difficulty is required.")
                .Must(d => new[] { "easy", "medium", "hard" }.Contains(d.ToLowerInvariant())).WithMessage("Problem difficulty invalid.");
            RuleFor(x => x.TimeLimitMs)
                .NotEmpty().WithMessage("Problem time limit is required.")
                .Must(t => t > 0 && t < int.MaxValue).WithMessage("Problem time limit invalid.");
            RuleFor(x => x.MemoryLimitMb)
                .NotEmpty().WithMessage("Problem memory is required.")
                .Must(m => m >= 0 && m < int.MaxValue).WithMessage("Problem memory is invalid");
        }
    }
}
