using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Problem;

namespace MeetCode.Application.Commands.CommandValidators.Problem
{
    public class ProblemDeleteCommandValidator : AbstractValidator<ProblemDeleteCommand>
    {
        public ProblemDeleteCommandValidator()
        {
            RuleFor(x => x.ProblemId)
                .NotEmpty().WithMessage("ProblemId is required.");
        }
    }
}
