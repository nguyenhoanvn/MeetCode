using MeetCode.Application.DTOs.Response.ProblemTemplate;
using MeetCode.Application.DTOs.Response.Tag;
using MeetCode.Application.DTOs.Response.TestCase;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.DTOs.Response.Problem
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
        List<TestCaseResponse> TestCaseList,
        List<ProblemTemplateResponse> TemplateList
        );
}
