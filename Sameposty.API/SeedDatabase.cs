using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;
using Sameposty.Services.Hasher;
using Sameposty.Services.Secrets;

namespace Sameposty.API;

public class SeedDatabase(SamepostyDbContext db, ISecretsProvider secrets)
{
    public async Task Run()
    {
        if (!db.Users.Any(x => x.Email == "Admin"))
        {
            var salt = Hasher.GetSalt();
            var passwordHashed = Hasher.HashPassword(secrets.AdminPassword, salt);

            var user = new User
            {
                Email = "Admin",
                Password = passwordHashed,
                Salt = salt,
                NIP = "5321574227",
                Name = "Admin Company",
                Street = "Admin Street",
                City = "Admin City",
                BuildingNumber = "1",
                FlatNumber = "1",
                PostCode = "00-000",
                REGON = "123456789",
                IsSuspended = false,
                IsActive = true,
                Role = Roles.Admin,
                IsVerified = true,
                ImageTokensLeft = 10000,
                TextTokensLeft = 10000,
            };

            await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
        }
    }
}
