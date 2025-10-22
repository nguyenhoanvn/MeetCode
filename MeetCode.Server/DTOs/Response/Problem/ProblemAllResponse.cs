namespace MeetCode.Server.DTOs.Response.Problem
{
    public sealed record ProblemAllResponse(IEnumerable<ProblemResponse> ProblemList);
}
