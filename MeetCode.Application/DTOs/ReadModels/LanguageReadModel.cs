using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.ReadModels
{
    public sealed record LanguageReadModel(
        Guid LangId,
        string Name,
        string Version,
        string FileExtension,
        string CompileImage,
        string RuntimeImage,
        string? CompileCommand,
        string? RunCommand,
        bool IsEnabled,
        int SubmissionCount,
        int ProblemTemplateCount
        );
}
