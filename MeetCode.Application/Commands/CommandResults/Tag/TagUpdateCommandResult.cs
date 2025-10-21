using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Commands.CommandResults.Tag
{
    public sealed record TagUpdateCommandResult(ProblemTag Tag);
}
