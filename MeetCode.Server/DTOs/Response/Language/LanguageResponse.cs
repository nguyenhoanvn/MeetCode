using MeetCode.Domain.Entities;

namespace MeetCode.Server.DTOs.Response.Language
{
    public sealed record LanguageResponse(
        string Name,
        string Version,
        string RuntimeImage,
        string? CompileCommand,
        string? RunCommand,
        bool IsEnabled
        );
}
