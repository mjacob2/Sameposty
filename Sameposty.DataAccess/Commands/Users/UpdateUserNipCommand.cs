using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Users;
public class UpdateUserNipCommand(int userId, string newNip) : CommandBase<User, User>
{
    public override async Task<User> Execute(SamepostyDbContext db)
    {
        var user = await db.Users.FindAsync(userId) ?? throw new Exception($"id: {userId}. Nie ma takiego użytkownika");

        user.NIP = newNip;

        db.Users.Update(user);
        await db.SaveChangesAsync();
        return user;
    }
}
