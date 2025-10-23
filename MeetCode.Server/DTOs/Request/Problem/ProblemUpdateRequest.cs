namespace MeetCode.Server.DTOs.Request.Problem
{
    public sealed record ProblemUpdateRequest(
        string NewTitle,
        string NewStatementMd,
        string NewDifficulty,
        IEnumerable<Guid> TagIds
        );
}
