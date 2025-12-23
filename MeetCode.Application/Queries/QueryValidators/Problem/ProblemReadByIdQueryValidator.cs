using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Queries.QueryEntities.Problem;

namespace MeetCode.Application.Queries.QueryValidators.Problem
{
    public class ProblemReadByIdQueryValidator : AbstractValidator<ProblemReadByIdQuery>
    {
        public ProblemReadByIdQueryValidator()
        {
            RuleFor(x => x.ProblemId)
                .NotEmpty().WithMessage("Problem Id is required.")
                .Must(x => x.GetType() == typeof(Guid)).WithMessage("Problem Id type mismatch.");
        }
    }
}
