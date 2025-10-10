using Microsoft.AspNetCore.Identity;
using ReactASP.Application.Interfaces.Services;
using ReactASP.Domain.Entities;
using ReactASP.Infrastructure.Persistence;

namespace ReactASP.Server.Helpers
{
    public static class AppDbInit
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider, CancellationToken ct)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var users = configuration.GetSection("Seeding:Accounts").Get<List<User>>();
            if (users == null || !users.Any())
                return;
            foreach (var user in users)
            {
                var existingUser = await context.Users.FindAsync(user.UserId, ct);
            
                if (existingUser == null)
                {
                    var newUser = new User
                    {
                        Email = user.Email,
                        DisplayName = user.DisplayName,
                        PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash),
                        Role = user.Role
                    };
                    var result = await context.AddAsync(newUser, ct); 
                }               
            }
            await context.SaveChangesAsync(ct);
        }
    }
}
