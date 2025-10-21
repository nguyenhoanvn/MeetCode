using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Tag;
using MeetCode.Application.Commands.CommandResults.Tag;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Commands.CommandHandlers.Tag
{
    public class TagAddCommandHandler : IRequestHandler<TagAddCommand, Result<TagAddCommandResult>>
    {
        private readonly ILogger<TagAddCommandHandler> _logger;
        private readonly ITagService _tagService;
        public TagAddCommandHandler(
            ILogger<TagAddCommandHandler> logger,
            ITagService tagService
            )
        {
            _logger = logger;
            _tagService = tagService;
        }

        public async Task<Result<TagAddCommandResult>> Handle(TagAddCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to add new tag with name {request.Name}");
            if (request == null)
            {
                _logger.LogWarning($"Add new tag failed because request is null");
                return Result.Error($"Add new tag failed because request is null");
            }

            var tag = await _tagService.CreateTagAsync(request.Name, ct);
            
            _logger.LogInformation($"Tag {tag.Name} added successfully");

            var result = new TagAddCommandResult(tag);
            return Result.Success(result);
        }
    }
}
