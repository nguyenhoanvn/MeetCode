using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Tag;
using MeetCode.Application.Queries.QueryResults.Tag;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Queries.QueryHandlers.Tag
{
    public class TagReadQueryHandler : IRequestHandler<TagReadQuery, Result<TagReadQueryResult>>
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagReadQueryHandler> _logger;
        public TagReadQueryHandler(
            ITagService tagService,
            ILogger<TagReadQueryHandler> logger)
        {
            _tagService = tagService;
            _logger = logger;
        }

        public async Task<Result<TagReadQueryResult>> Handle(TagReadQuery request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to retrieve tag {request.TagId}");
            var tag = await _tagService.FindTagByIdAsync(request.TagId, ct);
            if (tag == null)
            {
                _logger.LogWarning($"Tag with id {request.TagId} does not exist");
                return Result.NotFound($"Tag with id {request.TagId} does not exist");
            }

            var result = new TagReadQueryResult(tag);
            return Result.Success(result);
        }
    }
}
