namespace MeetCode.Application.DTOs.Request.TestCase
{
    public sealed record TestCaseAddRequest(
        string Visibility,
        string InputText,
        string OutputText,
        int Weight
        );
}
