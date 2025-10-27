using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandResults.TestCase
{
    public sealed record TestCaseAddCommandResult(MeetCode.Domain.Entities.TestCase TestCase);
}
