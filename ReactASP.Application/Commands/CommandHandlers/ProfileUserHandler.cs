using MediatR;
using ReactASP.Application.Commands.CommandEntities;
using ReactASP.Application.Commands.Profile;
using ReactASP.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.CommandHandlers
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
