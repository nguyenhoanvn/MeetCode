using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Language;
using MeetCode.Application.Interfaces.Services;

namespace MeetCode.Application.Commands.CommandValidators.Language
{
    public class LanguageUpdateCommandValidator : AbstractValidator<LanguageUpdateCommand>
    {
        public LanguageUpdateCommandValidator(IDockerValidator dockerValidator)
        {
            RuleFor(x => x)
                .MustAsync(async (x, ct) =>
                {
                    return await dockerValidator.ImageExistsAsync(x.RuntimeImage, ct);
                })
                .WithMessage(x => $"Docker image for {x.Name}:{x.RuntimeImage} does not exist.");
        }
    }


}
