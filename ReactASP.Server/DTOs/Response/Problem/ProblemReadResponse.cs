namespace ReactASP.Server.DTOs.Response.Problem
{
    public sealed record ProblemReadResponse(
        string Title,
        string StatementMd,
        string Difficulty,
        int TotalSubmissionCount,
        int ScoreAcceptedCount,
        double AcceptanceRate
        );
}
