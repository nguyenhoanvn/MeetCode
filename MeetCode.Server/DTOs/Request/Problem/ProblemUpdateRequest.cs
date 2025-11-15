namespace MeetCode.Server.DTOs.Request.Problem
{
    public sealed record ProblemUpdateRequest(
        string NewStatementMd,
        string NewDifficulty,
        IEnumerable<Guid> TagIds
        );
}
