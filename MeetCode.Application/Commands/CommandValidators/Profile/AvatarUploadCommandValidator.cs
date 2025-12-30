using FluentValidation;
using MeetCode.Application.Commands.CommandEntities.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandValidators.Profile
{
    public class AvatarUploadCommandValidator : AbstractValidator<AvatarUploadCommand>
    {
        private const long MaxFileSize = 5 * 1024 * 1024;
        private static readonly string[] AllowedExtensions  = { ".jpg", ".jpeg", ".png" };
        private static readonly string[] AllowedContentTypes = {
            "image/jpeg",
            "image/png"
        };
        public AvatarUploadCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("UserId is required")
                .Must(x => x.GetType() == typeof(Guid)).WithMessage("UserId type mismatch");
            RuleFor(x => x.File)
                .NotNull().WithMessage("Please import a file")
                .Must(x => x.Length > 0).WithMessage("No file provided")
                .Must(x => x.Length <= MaxFileSize).WithMessage("File size cannot be exceeds 5Mb")
                .Must(x => AllowedExtensions.Contains(Path.GetExtension(x.FileName).ToLowerInvariant())).WithMessage("Invalid file type. Only .jpg, .jpeg and .png are allowed")
                .Must(x => AllowedContentTypes.Contains(x.ContentType)).WithMessage("Invalid file content type");
        }
    }
}
