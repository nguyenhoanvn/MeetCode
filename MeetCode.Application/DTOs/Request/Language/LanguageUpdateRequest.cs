namespace MeetCode.Application.DTOs.Request.Language
{
    public sealed record LanguageUpdateRequest(
        string Version,
        string RuntimeImage,
        string CompileCommand,
        string RunCommand
        );
}
