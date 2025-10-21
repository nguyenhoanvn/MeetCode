using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MediatR;
using MeetCode.Application.Commands.CommandResults.Tag;

namespace MeetCode.Application.Commands.CommandEntities.Tag
{
    public sealed record TagDeleteCommand(Guid TagId) : IRequest<Result<TagDeleteCommandResult>>;
}
