using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Application.Queries.QueryEntities.Tag;
using MeetCode.Application.Queries.QueryResults.Tag;
using Microsoft.Extensions.Logging;

namespace MeetCode.Application.Queries.QueryHandlers.Tag
{
    public class TagReadByIdQueryHandler : IRequestHandler<TagReadByIdQuery, Result<TagReadQueryResult>>
    {
        private readonly ITagRepository _tagRepository;
        private readonly ILogger<TagReadByIdQueryHandler> _logger;
        public TagReadByIdQueryHandler(
            ITagRepository tagRepository,
            ILogger<TagReadByIdQueryHandler> logger)
        {
            _tagRepository = tagRepository;
            _logger = logger;
        }

        public async Task<Result<TagReadQueryResult>> Handle(TagReadByIdQuery request, CancellationToken ct)
        {
            _logger.LogInformation($"Attempting to retrieve tag {request.TagId}");
            var tag = await _tagRepository.GetByIdAsync(request.TagId, ct);
            if (tag == null)
            {
                _logger.LogWarning("Tag with id {TagId} does not exist", request.TagId);
                return Result.NotFound($"Tag with id {request.TagId} does not exist");
            }

            return Result.Success(new TagReadQueryResult(tag));
        }
    }
}
