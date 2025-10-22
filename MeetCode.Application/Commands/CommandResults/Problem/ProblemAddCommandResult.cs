using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Commands.CommandResults.Problem
{
    public sealed record ProblemAddCommandResult(MeetCode.Domain.Entities.Problem Problem
        );
}
