using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ardalis.Result;
using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<User>> FindUserByEmailAsync(string email, CancellationToken ct);
        bool IsPasswordMatch(string requestPassword, string userDbPassword);
        Task<User?> FindUserAsync(Guid userId, CancellationToken ct);
        string HashPassword(string plainPassword);
        Task<Result<User>> CreateUserAsync(string email, string displayName, string plainPassword, CancellationToken ct);
    }
}
