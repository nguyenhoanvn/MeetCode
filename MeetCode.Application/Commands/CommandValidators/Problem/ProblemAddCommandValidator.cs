using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Problem;

namespace MeetCode.Application.Commands.CommandValidators.Problem
{
    public sealed class ProblemAddCommandValidator : AbstractValidator<ProblemAddCommand>
    {
        public ProblemAddCommandValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Problem title is required.")
                .MaximumLength(255).WithMessage("Problem title must less than 255 characters.")
                .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Problem must contain only alphabetical/numeric characters.");
            RuleFor(x => x.StatementMd)
                .NotEmpty().WithMessage("Problem statement is required");
            RuleFor(x => x.Difficulty)
                .NotEmpty().WithMessage("Problem difficulty is required.")
                .Must(d => new[] { "easy", "medium", "hard" }.Contains(d.ToLowerInvariant())).WithMessage("Problem difficulty must be either \"easy\", \"medium\" or \"hard\".");
            RuleFor(x => x.TimeLimitMs)
                .NotEmpty().WithMessage("Problem time limit is required.")
                .Must(t => t > 0 && t < int.MaxValue).WithMessage("Problem time limit must be a positive number");
            RuleFor(x => x.MemoryLimitMb)
                .NotEmpty().WithMessage("Problem memory is required.")
                .Must(m => m >= 0 && m < int.MaxValue).WithMessage("Problem memory limit must be a positive number");
        }
    }
}
