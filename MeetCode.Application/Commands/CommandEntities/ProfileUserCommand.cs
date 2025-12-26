using MediatR;
using MeetCode.Application.Commands.CommandResults.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandEntities
{
    public sealed record ProfileUserCommand() : IRequest<ProfileUserResult>;
}
