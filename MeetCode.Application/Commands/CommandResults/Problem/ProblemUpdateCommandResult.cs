using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandResults.Problem
{
    public sealed record ProblemUpdateCommandResult(Domain.Entities.Problem UpdatedProblem);
}
