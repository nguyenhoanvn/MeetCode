using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Language;

namespace MeetCode.Application.Commands.CommandValidators.Language
{
    public class LanguageUpdateCommandValidator : AbstractValidator<LanguageUpdateCommand>
    {
        public LanguageUpdateCommandValidator(IDockerValidator dockerValidator)
        {
            RuleFor(x => x)
                .MustAsync(async (x, ct) =>
                {
                    var imagePattern = GetImagePattern(x.Name);
                    var image = imagePattern.Replace("{version}", x.Version);

                    return await dockerValidator.ImageExistsAsync(image, ct);
                })
                .WithMessage(x => $"Docker image for {x.Name}:{x.Version} does not exist.");
        }

        private string GetImagePattern(string languageName)
        {

            switch (languageName.ToLower())
            {
                case "c#":
                    return "mcr.microsoft.com/dotnet/sdk:{version}";
                case "java":
                    return "openjdk:{version}-jdk-slim";
                default:
                    throw new InvalidOperationException($"Unsupported language: {languageName}");
            };
        }
    }


}
