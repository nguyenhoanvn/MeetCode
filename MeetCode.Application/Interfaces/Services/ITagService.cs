using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Services
{
    public interface ITagService
    {
        Task<ProblemTag> CreateTagAsync(string name, CancellationToken ct);
        Task<ProblemTag?> FindTagByIdAsync(Guid tagId, CancellationToken ct);
        Task<IEnumerable<ProblemTag>> ReadAllTagsAsync(CancellationToken ct);
        Task<ProblemTag?> FindTagByNameAsync(string name, CancellationToken ct);
        Task<ProblemTag?> UpdateTagAsync(ProblemTag newTag, CancellationToken ct);
        Task DeleteTagAsync(ProblemTag tagToDelete, CancellationToken ct);

    }
}
