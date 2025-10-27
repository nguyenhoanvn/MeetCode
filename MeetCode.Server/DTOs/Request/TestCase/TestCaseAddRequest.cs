namespace MeetCode.Server.DTOs.Request.TestCase
{
    public sealed record TestCaseAddRequest(
        string Visibility,
        string InputText,
        string OutputText,
        int Weight
        );
}
