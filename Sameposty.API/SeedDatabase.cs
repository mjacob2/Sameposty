using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.API;

public class SeedDatabase(SamepostyDbContext db)
{
    public async Task Run()
    {
        if (!db.Users.Any())
        {
            var user = new User
            {
                Email = "jakubicki.m@gmail.com",
                Password = "adminadmin",
                Salt = string.Empty,
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
                ImageTokensLimit = 1000,
                TextTokensLimit = 1000,
            };

            db.Users.Add(user);
            db.SaveChanges();
        }
    }
}
