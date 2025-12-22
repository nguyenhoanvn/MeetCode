using FluentValidation;
using MeetCode.Application.Queries.QueryEntities.ProblemTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryValidators.ProblemTemplate
{
    public class ProblemTemplateReadBySlugQueryValidator : AbstractValidator<ProblemTemplateReadBySlugQuery>
    {
        public ProblemTemplateReadBySlugQueryValidator()
        {

        }
    }
}
