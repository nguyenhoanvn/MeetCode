using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReactASP.Domain.Entities;

namespace ReactASP.Application.Commands.CommandResults.Problem
{
    public record class ProblemAllResult(IEnumerable<ReactASP.Domain.Entities.Problem> ProblemList);
}
