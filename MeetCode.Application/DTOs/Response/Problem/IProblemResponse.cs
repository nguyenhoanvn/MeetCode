using MeetCode.Application.DTOs.Response.Tag;
using MeetCode.Application.DTOs.Response.TestCase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Response.Problem
{
    public interface IProblemResponse
    {
        Guid ProblemId { get; }
        string Title { get; }
        string Slug { get; }
        string StatementMd { get; }
        string Difficulty { get; }
        int TotalSubmissionCount { get; }
        int ScoreAcceptedCount { get; }
        double? AcceptanceRate { get; }
        List<TagResponse> TagList { get; }
        List<TestCaseResponse> TestCaseList { get; }
    }
}
