using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Queries.QueryEntities.Tag;

namespace MeetCode.Application.Queries.QueryValidators.Tag
{
    public class TagReadQueryValidator : AbstractValidator<TagReadQuery>
    {
        public TagReadQueryValidator()
        {
            RuleFor(x => x.TagId)
                .NotEmpty().WithMessage("Tag ID is required.")
                .Must(x => x.GetType() == typeof(Guid));
        }
    }
}
