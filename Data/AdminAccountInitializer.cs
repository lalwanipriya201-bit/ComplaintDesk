
using ComplaintMgtSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ComplaintMgtSystem.Data
{
    public static class AdminAccountInitializer
    {
        public static async Task InitializeAsync(
            IServiceProvider services,
            IConfiguration configuration)
        {
            using var scope = services.CreateScope();

            var context = scope.ServiceProvider
                .GetRequiredService<ApplicationDbContext>();

            var password = configuration["AdminAccount:Password"];
            var name = configuration["AdminAccount:Name"];
            var email = configuration["AdminAccount:Email"];

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new InvalidOperationException(
                    "Set AdminAccount__Password before starting the application.");
            }

            if (string.IsNullOrWhiteSpace(name) ||
                string.IsNullOrWhiteSpace(email))
            {
                throw new InvalidOperationException(
                    "Admin account name and email must be configured.");
            }

            email = email.Trim();

            var existingAdmin = await context.Users
                .FirstOrDefaultAsync(u =>
                    u.Email.ToLower() == email.ToLower());

            if (existingAdmin != null)
            {
                if (existingAdmin.Role != "Admin")
                {
                    throw new InvalidOperationException(
                        "The configured admin email belongs to a non-admin account.");
                }

                return;
            }

            var admin = new User
            {
                Name = name.Trim(),
                Email = email,
                Role = "Admin",
                CreatedAt = DateTime.UtcNow
            };

            var hasher = new PasswordHasher<User>();
            admin.PasswordHash = hasher.HashPassword(admin, password);

            context.Users.Add(admin);
            await context.SaveChangesAsync();
        }
    }
}
