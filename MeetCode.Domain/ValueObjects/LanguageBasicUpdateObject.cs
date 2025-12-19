using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Domain.ValueObjects
{
    public sealed record LanguageBasicUpdateObject(
        string? Name,
        string? Version,
        string? FileExtension,
        string? CompileImage,
        string? RuntimeImage,
        string? CompileCommand,
        string? RunCommand
    );
}
