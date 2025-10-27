using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Application.Commands.CommandHandlers.Tag;
using MeetCode.Application.Interfaces.Repositories;
using MeetCode.Application.Interfaces.Services;
using MeetCode.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace MeetCode.Infrastructure.Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<TagService> _logger;
        public TagService(
            ITagRepository tagRepository,
            IUnitOfWork unitOfWork,
            ILogger<TagService> logger)
        {
            _tagRepository = tagRepository;
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ProblemTag> CreateTagAsync(string name, CancellationToken ct)
        {
            var existing = await _tagRepository.GetByNameAsync(name, ct);
            if (existing != null)
            {
                throw new InvalidOperationException($"Tag {name} already exists");
            }
            var tag = new ProblemTag
            {
                TagId = Guid.NewGuid(),
                Name = name
            };

            await _tagRepository.AddAsync(tag, ct);
            await _unitOfWork.SaveChangesAsync(ct);

            return tag;
        }
        public async Task<ProblemTag?> FindTagByIdAsync(Guid tagId, CancellationToken ct)
        {
            return await _tagRepository.GetByIdAsync(tagId, ct);
        }
        public async Task<IEnumerable<ProblemTag>> ReadAllTagsAsync(CancellationToken ct)
        {
            return await _tagRepository.GetAsync(ct);
        }
        public async Task<ProblemTag?> FindTagByNameAsync(string name, CancellationToken ct)
        {
            return await _tagRepository.GetByNameAsync(name, ct);
        }

        public async Task<ProblemTag?> UpdateTagAsync(ProblemTag newTag, CancellationToken ct)
        {
            await _tagRepository.Update(newTag);
            await _unitOfWork.SaveChangesAsync(ct);
            return newTag;
        }

        public async Task DeleteTagAsync(ProblemTag problemToDelete, CancellationToken ct)
        {
            await _tagRepository.Delete(problemToDelete);
            await _unitOfWork.SaveChangesAsync(ct);
        }

        public async Task<IEnumerable<ProblemTag>> GetInRange(IEnumerable<Guid> tagIds, CancellationToken ct)
        {
            return await _tagRepository.GetByIdsAsync(tagIds, ct);
        }
    }
}
