using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Tag;

namespace MeetCode.Application.Commands.CommandValidators.Tag
{
    public class TagAddCommandValidator : AbstractValidator<TagAddCommand>
    {
        public TagAddCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required");
        }
    }
}
