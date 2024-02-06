using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Users;
public class AddUserCommand : CommandBase<User, User>
{
    public override async Task<User> Execute(SamepostyDbContext db)
    {
        await db.Users.AddAsync(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
