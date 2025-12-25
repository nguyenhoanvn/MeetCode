using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.ProblemTemplate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandEntities.ProblemTemplate
{
    public sealed record ProblemTemplateAddCommand(
        Guid ProblemId,
        Guid LangId,
        string MethodName,
        string ReturnType,
        string[] Parameters,
        string? CompileCommand = default,
        string? RunCommand = default
        ) : IRequest<Result<ProblemTemplateAddCommandResult>>;
}
