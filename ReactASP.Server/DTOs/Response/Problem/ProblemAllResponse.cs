namespace ReactASP.Server.DTOs.Response.Problem
{
    public sealed record ProblemAllResponse(IEnumerable<ReactASP.Domain.Entities.Problem> ProblemList);
}
