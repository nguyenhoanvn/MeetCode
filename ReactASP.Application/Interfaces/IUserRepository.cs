using ReactASP.Domain.Entities;

namespace ReactASP.Application.Interfaces;

public interface IUserRepository {
    Task<bool> EmailExistsAsync(string email, CancellationToken ct);
    Task AddAsync(User user, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken ct);
    Task<User?> FindUserAsync(Guid userId, CancellationToken ct);

}