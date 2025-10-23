using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandEntities.Tag;
using MeetCode.Application.Commands.CommandResults.Tag;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Commands.CommandHandlers.Tag
{
    public class TagUpdateCommandHandler : IRequestHandler<TagUpdateCommand, Result<TagUpdateCommandResult>>
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagUpdateCommandHandler> _logger;
        public TagUpdateCommandHandler(
            ITagService tagService,
            ILogger<TagUpdateCommandHandler> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        public async Task<Result<TagUpdateCommandResult>> Handle(TagUpdateCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to update tag {request.TagId}");

            var tag = await _tagService.FindTagByIdAsync(request.TagId, ct);
            if (tag == null)
            {
                _logger.LogWarning($"No existing tag with id {request.TagId}");
                return Result.NotFound($"No existing tag with id {request.TagId}");
            }

            tag.Name = request.NewTagName;

            await _tagService.UpdateTagAsync(tag, ct);
            _logger.LogInformation($"Tag {request.NewTagName} updated successfully ");
            var result = new TagUpdateCommandResult(tag);
            return Result.Success(result);
        }
    }
}
