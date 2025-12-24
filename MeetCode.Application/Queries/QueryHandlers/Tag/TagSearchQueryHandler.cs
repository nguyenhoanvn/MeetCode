using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Queries.QueryEntities.Tag;
using MeetCode.Application.Queries.QueryResults.Tag;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Queries.QueryHandlers.Tag
{
    public class TagSearchQueryHandler : IRequestHandler<TagSearchQuery, Result<TagSearchQueryResult>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<TagSearchQueryHandler> _logger;
        public TagSearchQueryHandler(
            ITagRepository tagRepository,
            ILogger<TagSearchQueryHandler> logger
            )
        {
            _tagRepository = tagRepository;
            _logger = logger;
        }

        public async Task<Result<TagSearchQueryResult>> Handle(TagSearchQuery request, CancellationToken ct)
        {
            _logger.LogInformation("Attempting to search tag contain {Name}", request.TagName);

            var tags = await _tagRepository.GetAllContainNameAsync(request.TagName, ct);

            if (tags.Count() == 0)
            {
                _logger.LogWarning("Tag list is empty");
            }

            _logger.LogInformation("Tag list containing {Name} read with size {Size}", request.TagName, tags.Count());

            return Result.Success(new TagSearchQueryResult(tags.ToList()));
        }
    }
}
