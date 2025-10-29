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
    public sealed record TestCaseUpdateCommand(
        Guid TestCaseId,
        string NewVisibility,
        string NewInputText,
        string NewExpectedOutputText,
        int NewWeight) : IRequest<Result<TestCaseUpdateCommandResult>>;
}
