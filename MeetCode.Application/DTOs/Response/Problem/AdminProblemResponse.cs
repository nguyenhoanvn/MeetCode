using MeetCode.Application.DTOs.Response.ProblemTemplate;
using MeetCode.Application.DTOs.Response.Tag;
using MeetCode.Application.DTOs.Response.TestCase;

namespace MeetCode.Application.DTOs.Response.Problem
{
    public sealed record AdminProblemResponse(
        Guid ProblemId,
        string Title,
        string Slug,
        string StatementMd,
        string Difficulty,
        int TotalSubmissionCount,
        int ScoreAcceptedCount,
        double? AcceptanceRate,
        List<TagResponse> TagList,
        List<TestCaseResponse> TestCaseList,
        List<ProblemTemplateResponse> TemplateList,
        bool IsActive, 
        Guid CreatedBy 
    ) : IProblemResponse;
}
