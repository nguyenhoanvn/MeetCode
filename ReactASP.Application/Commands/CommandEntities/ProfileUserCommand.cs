using MediatR;
using ReactASP.Application.Commands.Profile;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.CommandEntities
{
    public sealed record ProfileUserCommand() : IRequest<ProfileUserResult>;
}
