namespace MeetCode.Application.DTOs.Response.Problem
{
    public sealed record ProblemAllResponse(
        List<ProblemResponse> ProblemList,
        int PageNumber,
        int PageSize,
        int TotalCount,
        int TotalPages
        );
}
