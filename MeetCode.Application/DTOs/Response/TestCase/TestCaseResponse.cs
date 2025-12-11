namespace MeetCode.Application.DTOs.Response.TestCase
{
    public sealed record TestCaseResponse(
        Guid TestId,
        string Visibility,
        string InputJson,
        string Output,
        int Weight,
        Guid ProblemId
        );
}
