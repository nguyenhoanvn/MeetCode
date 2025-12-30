using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Profile;
using MeetCode.Application.Commands.CommandResults.Profile;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers.Profile
{
    public class AvatarUploadCommandHandler : IRequestHandler<AvatarUploadCommand, Result<AvatarUploadCommandResult>>
    {
        private readonly IUserRepository _userRepository;
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<AvatarUploadCommandHandler> _logger;
        

        public AvatarUploadCommandHandler(
            IUserRepository userRepository,
            IFileStorageService fileStorageService,
            ILogger<AvatarUploadCommandHandler> logger
            )
        {
            _userRepository = userRepository;
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        public async Task<Result<AvatarUploadCommandResult>> Handle(AvatarUploadCommand request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to upload new avatar for user {UserId}", request.UserId);

            var user = await _userRepository.GetByIdAsync(request.UserId, ct);
            if (user == null)
            {
                _logger.LogWarning("Cannot find user with Id {UserId}", request.UserId);
                return Result.Invalid(new ValidationError(nameof(request.UserId), "User does not exist"));
            }

            var storagePath = await _fileStorageService.UploadAsync(request.File, $"avatars/{request.UserId}", ct);

            return Result.Success(new AvatarUploadCommandResult(storagePath));
        }
    }
}
