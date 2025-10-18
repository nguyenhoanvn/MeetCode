using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Problem;

namespace MeetCode.Application.Commands.CommandValidators.Problem
{
    public class ProblemUpdateCommandValidator : AbstractValidator<ProblemUpdateCommand>
    {
        public ProblemUpdateCommandValidator()
        {
            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$").WithMessage("Invalid slug input");
            RuleFor(x => x.NewStatementMd)
                .NotEmpty().WithMessage("Problem statement is required");
            RuleFor(x => x.NewTitle)
                .NotEmpty().WithMessage("Problem title is required.")
                .MaximumLength(255).WithMessage("Problem title must less than 255 characters.")
                .Matches("^[a-zA-Z0-9 ]+$").WithMessage("Problem must contain only alphabetical/numeric characters.");
            RuleFor(x => x.NewDifficulty)
                .NotEmpty().WithMessage("Problem difficulty is required.")
                .Must(d => new[] { "easy", "medium", "hard" }.Contains(d.ToLowerInvariant())).WithMessage("Problem difficulty must be either \"easy\", \"medium\" or \"hard\".");
        }
    }
}
