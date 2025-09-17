using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ReactASP.Infrastructure.Persistence;
using ReactASP.Domain.Entities;
using ReactASP.Application.Interfaces;

namespace ReactASP.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db)
        {
            _db = db;
        }

        public Task<bool> EmailExistsAsync(string email, CancellationToken ct)
        {
            return _db.Users.AnyAsync(u => u.Email == email, ct);
        }

        public async Task AddAsync(User user, CancellationToken ct)
        {
            await _db.Users.AddAsync(user, ct);
        }

        public Task SaveChangesAsync(CancellationToken ct)
        {
            return _db.SaveChangesAsync(ct);
        }
        public Task<User?> GetUserByEmailAsync(string email, CancellationToken ct)
        {
            return _db.Users.FirstOrDefaultAsync(u => u.Email == email, ct);
        }

        public Task<User?> FindUserAsync(Guid userId, CancellationToken ct)
        {
            return _db.Users.FindAsync(new object[] {userId}, ct).AsTask();
        }

    }
}
