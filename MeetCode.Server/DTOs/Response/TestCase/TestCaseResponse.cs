namespace MeetCode.Server.DTOs.Response.TestCase
{
    public sealed record TestCaseResponse(
        Guid TestId,
        string Visibility,
        string InputText,
        string OutputText,
        int Weight,
        Guid ProblemId
        );
}
