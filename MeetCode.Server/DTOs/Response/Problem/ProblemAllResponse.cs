namespace MeetCode.Server.DTOs.Response.Problem
{
    public sealed record ProblemAllResponse(IEnumerable<Domain.Entities.Problem> ProblemList);
}
