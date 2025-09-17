using MediatR;
using ReactASP.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReactASP.Application.Commands.Profile
{
    public sealed class ProfileUserCommandHandler : IRequestHandler<ProfileUserCommand, ProfileUserResult>
    {
        private readonly IUserRepository _userRepository;

        public ProfileUserCommandHandler(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<ProfileUserResult> Handle(ProfileUserCommand request, CancellationToken ct)
        {
            return null;
        }
    }
}
