using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Queries.QueryEntities.Problem;

namespace MeetCode.Application.Queries.QueryValidators.Problem
{
    public class ProblemReadQueryValidator : AbstractValidator<ProblemReadQuery>
    {
        public ProblemReadQueryValidator()
        {
            RuleFor(x => x.ProblemSlug)
                .NotEmpty().WithMessage("Problem slug is required.");
        }
    }
}
