using MediatR;
using MeetCode.Application.Commands.CommandEntities;
using MeetCode.Application.Commands.CommandResults;
using MeetCode.Application.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetCode.Application.Commands.CommandHandlers
{
    public sealed class ProfileUserHandler : IRequestHandler<ProfileUserCommand, ProfileUserResult>
    {
        private readonly IUserRepository _userRepository;

        public ProfileUserHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ProfileUserResult> Handle(ProfileUserCommand request, CancellationToken ct)
        {
            return null;
        }
    }
}
