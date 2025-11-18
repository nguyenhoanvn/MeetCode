namespace MeetCode.Server.DTOs.Response.TestCase
{
    public sealed record TestCaseResponse(
        Guid TestId,
        string Visibility,
        string InputJson,
        string OutputJson,
        int Weight,
        Guid ProblemId
        );
}
