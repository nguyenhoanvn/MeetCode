using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.DTOs.Request.ProblemTemplate
{
    public sealed record ProblemTemplateAddRequest(
        Guid ProblemId,
        Guid LangId,
        string MethodName,
        string ReturnType,
        string[] Parameters,
        string? CompileCommand,
        string? RunCommand
        );
}
