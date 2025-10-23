using MeetCode.Domain.Entities;

namespace MeetCode.Server.DTOs.Request.Problem
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
