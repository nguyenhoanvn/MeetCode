namespace ReactASP.Server.DTOs.Response.Problem
{
    public sealed record ProblemAddResponse(
        Guid ProblemId,
        string Slug,
        string Title,
        string Difficulty,
        DateTimeOffset CreatedAt
        );
}
