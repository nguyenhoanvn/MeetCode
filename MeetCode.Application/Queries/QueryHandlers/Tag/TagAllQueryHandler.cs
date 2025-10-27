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
    public class TagAllQueryHandler : IRequestHandler<TagAllQuery, Result<TagAllQueryResult>>
    {
        private readonly ITagService _tagService;
        private readonly ILogger<TagAllQueryHandler> _logger;
        public TagAllQueryHandler(
            ITagService tagService,
            ILogger<TagAllQueryHandler> logger
            )
        {
            _tagService = tagService;
            _logger = logger;
        }

        public async Task<Result<TagAllQueryResult>> Handle(TagAllQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to retrieve all tag");
            var tagList = await _tagService.ReadAllTagsAsync(ct);
            if (tagList.Count() == 0)
            {
                _logger.LogWarning("Cannot find any tag");
            }
            var result = new TagAllQueryResult(tagList);
            return Result.Success(result);
        }
    }
}
