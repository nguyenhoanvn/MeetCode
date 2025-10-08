using ReactASP.Application.Interfaces.Repositories;
using ReactASP.Domain.Entities;

namespace ReactASP.Application.Interfaces;

public interface IUserRepository : IRepository<User>{
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken ct);
    Task<User?> GetUserByEmailWithTokensAsync(string email, CancellationToken ct);
    Task<User?> GetUserByIdWithTokensAsync(Guid userId, CancellationToken ct);

}