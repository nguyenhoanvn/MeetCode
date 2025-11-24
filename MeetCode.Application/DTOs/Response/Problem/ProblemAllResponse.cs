namespace MeetCode.Application.DTOs.Response.Problem
{
    public sealed record ProblemAllResponse(List<ProblemResponse> ProblemList) : IProblemAllResponse;
}
