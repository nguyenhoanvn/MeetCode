using MeetCode.Server.DTOs.Response.Tag;

namespace MeetCode.Server.DTOs.Response.Problem
{
    public sealed record ProblemResponse(
        Guid ProblemId,
        string Title,
        string StatementMd,
        string Difficulty,
        int TotalSubmissionCount,
        int ScoreAcceptedCount,
        double? AcceptanceRate,
        List<TagResponse> TagList
        );
}
