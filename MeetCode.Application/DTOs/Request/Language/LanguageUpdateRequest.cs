namespace MeetCode.Application.DTOs.Request.Language
{
    public sealed record LanguageUpdateRequest(
        string? Name,
        string? Version,
        string? FileExtension,
        string? CompileImage,
        string? RuntimeImage,
        string? CompileCommand,
        string? RunCommand
        );
}
