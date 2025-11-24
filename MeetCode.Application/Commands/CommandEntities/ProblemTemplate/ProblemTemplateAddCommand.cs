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
        string MethodName,
        string ReturnType,
        string[] Parameters,
        Guid ProblemId,
        Guid LangId
        ) : IRequest<Result<ProblemTemplateAddResult>>;
}
