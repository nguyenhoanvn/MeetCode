using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.TestCase;

namespace MeetCode.Application.Commands.CommandEntities.TestCase
{
    public sealed record TestCaseAddCommand(
        Guid ProblemId,
        string Visibility,
        string InputText,
        string ExpectedOutputText,
        int Weight
        ) : IRequest<Result<TestCaseAddCommandResult>>;
}
