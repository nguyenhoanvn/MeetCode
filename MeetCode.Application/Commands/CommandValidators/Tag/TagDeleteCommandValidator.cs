using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Tag;

namespace MeetCode.Application.Commands.CommandValidators.Tag
{
    public class TagDeleteCommandValidator : AbstractValidator<TagDeleteCommand>
    {
        public TagDeleteCommandValidator()
        {
            RuleFor(x => x.TagId)
                .NotEmpty().WithMessage("Tag ID is required.")
                .Must(x => x.GetType() == typeof(Guid));
        }
    }
}
