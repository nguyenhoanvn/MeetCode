using MeetCode.Server.DTOs.Response.Tag;
using MeetCode.Server.DTOs.Response.TestCase;

namespace MeetCode.Server.DTOs.Response.Problem
{
    public sealed record ProblemResponse(
        Guid ProblemId,
        string Title,
        string Slug,
        string StatementMd,
        string Difficulty,
        int TotalSubmissionCount,
        int ScoreAcceptedCount,
        double? AcceptanceRate,
        List<TagResponse> TagList,
        List<TestCaseResponse> TestCaseList
        );
}
