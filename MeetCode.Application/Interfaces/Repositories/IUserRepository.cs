using MeetCode.Domain.Entities;

namespace MeetCode.Application.Interfaces.Repositories;

public interface IUserRepository : IRepository<User>{

    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetUserByEmailWithTokensAsync(string email, CancellationToken ct);
    Task<User?> GetUserByIdWithTokensAsync(Guid userId, CancellationToken ct);

}