namespace MeetCode.Server.DTOs.Request.TestCase
{
    public sealed record TestCaseUpdateRequest(
        string Visibility,
        string InputText,
        string ExpectedOutputText,
        int Weight);
}
