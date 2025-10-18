namespace MeetCode.Server.DTOs.Response.Problem
{
    public sealed record ProblemUpdateResponse(
        string Slug,
        string Title,
        string StatementMd,
        string Difficulty
        );
}
