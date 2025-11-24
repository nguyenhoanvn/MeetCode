using MeetCode.Domain.Entities;
using MeetCode.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using MeetCode.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;

namespace MeetCode.Server.Helpers
{
    public static class AppDbInit
    {
        public static async Task SeedUsersAndRolesAsync(IServiceProvider serviceProvider, CancellationToken ct)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var users = configuration.GetSection("Seeding:Accounts").Get<List<User>>();
            if (users == null || !users.Any())
            {
                return;
            }
            foreach (var user in users)
            {
                var existingUser = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Email == user.Email, ct);

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

        public static async Task SeedLanguagesAsync(IServiceProvider serviceProvider, CancellationToken ct)
        {
            var context = serviceProvider.GetRequiredService<AppDbContext>();
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var languages = configuration.GetSection("Seeding:Languages").Get<List<Language>>();
            if (languages == null || !languages.Any())
            {
                return;
            }
            foreach (var language in languages)
            {
                var existingLanguage = await context.Languages
                    .AsNoTracking()
                    .FirstOrDefaultAsync(l => l.Name == language.Name, ct);
                if (existingLanguage == null)
                {
                    var newLanguage = new Language
                    {
                        Name = language.Name,
                        Version = language.Version,
                        CompileImage = language.CompileImage,
                        RuntimeImage = language.RuntimeImage,
                        CompileCommand = language.CompileCommand,
                        RunCommand = language.RunCommand,
                        IsEnabled = language.IsEnabled
                    };
                    var result = await context.AddAsync(newLanguage, ct);
                }
            }
            await context.SaveChangesAsync(ct);
        }
    }
}
