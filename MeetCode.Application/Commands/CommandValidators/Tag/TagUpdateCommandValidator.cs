using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Tag;

namespace MeetCode.Application.Commands.CommandValidators.Tag
{
    public class TagUpdateCommandValidator : AbstractValidator<TagUpdateCommand>
    {
        public TagUpdateCommandValidator()
        {
            RuleFor(x => x.TagId)
                .NotEmpty().WithMessage("Tag ID is required.")
                .Must(x => x.GetType() == typeof(Guid));
            RuleFor(x => x.NewTagName)
                .NotEmpty().WithMessage("Tag Name is required.");
        }
    }
}
