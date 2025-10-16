using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.CommandResults.Problem
{
    public sealed record ProblemUpdateCommandResult(ReactASP.Domain.Entities.Problem UpdatedProblem);
}
