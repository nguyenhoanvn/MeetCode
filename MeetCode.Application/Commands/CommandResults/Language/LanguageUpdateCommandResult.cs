using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandResults.Language
{
    public sealed record LanguageUpdateCommandResult(MeetCode.Domain.Entities.Language Language);
}
