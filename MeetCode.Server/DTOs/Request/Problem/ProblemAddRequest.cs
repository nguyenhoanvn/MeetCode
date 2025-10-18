namespace MeetCode.Server.DTOs.Request.Problem
{
    public sealed record ProblemAddRequest(
        string Title, 
        string StatementMd,
        string Difficulty,
        int TimeLimitMs,
        int MemoryLimitMb
        );

}
