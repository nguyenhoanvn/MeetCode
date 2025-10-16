using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using ReactASP.Application.Queries.QueryEntities.Problem;

namespace ReactASP.Application.Queries.QueryValidators.Problem
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
