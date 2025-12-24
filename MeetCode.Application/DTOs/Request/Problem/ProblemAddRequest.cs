
using MeetCode.Application.DTOs.Request.ProblemTemplate;

namespace MeetCode.Application.DTOs.Request.Problem
{
    public sealed record ProblemAddRequest(
        string Title, 
        string StatementMd,
        string Difficulty,
        int TimeLimitMs,
        int MemoryLimitMb,
        List<Guid> TagIds
        );

}
