using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Services
{
    public interface IProfileService
    {
        Task<User?> GetCurrentUser(Guid userId, CancellationToken ct);
    }
}
