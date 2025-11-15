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
            RuleFor(x => x.ProblemId)
                .NotEmpty().WithMessage("Problem Id is required.");
            RuleFor(x => x.NewStatementMd)
                .NotEmpty().WithMessage("Problem statement is required");
            RuleFor(x => x.NewDifficulty)
                .NotEmpty().WithMessage("Problem difficulty is required.")
                .Must(d => new[] { "easy", "medium", "hard" }.Contains(d.ToLowerInvariant())).WithMessage("Problem difficulty must be either \"easy\", \"medium\" or \"hard\".");
        }
    }
}
