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
    public class TagDeleteCommandHandler : IRequestHandler<TagDeleteCommand, Result<TagDeleteCommandResult>>
    {
        private readonly ILogger<TagDeleteCommandHandler> _logger;
        private readonly ITagService _tagService;
        public TagDeleteCommandHandler(
            ILogger<TagDeleteCommandHandler> logger,
            ITagService tagService
            )
        {
            _logger = logger;
            _tagService = tagService;
        }

        public async Task<Result<TagDeleteCommandResult>> Handle(TagDeleteCommand request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to delete tag {request.TagId}");
            var tag = await _tagService.FindTagByIdAsync(request.TagId, ct);
            if (tag == null)
            {
                _logger.LogWarning($"Tag {request.TagId} does not exist");
                return Result.Error($"Tag {request.TagId} does not exist");
            }

            if (tag.Problems.Count() != 0)
            {
                _logger.LogWarning($"Tag {request.TagId} got relationship with another enity");
                return Result.Error($"Tag {request.TagId} got relationship with another enity");
            }

            await _tagService.DeleteTagAsync(tag, ct);
            _logger.LogInformation($"Tag {request.TagId} deleted successfully");

            var result = new TagDeleteCommandResult($"Tag {request.TagId} deleted successfully");
            return Result.Success(result);
        }
    }
}
