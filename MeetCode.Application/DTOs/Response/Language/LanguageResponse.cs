using MeetCode.Domain.Entities;

namespace MeetCode.Application.DTOs.Response.Language
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
