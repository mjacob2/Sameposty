using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Users;
public class UpdateUserCommand : CommandBase<User, User>
{
    public override async Task<User> Execute(SamepostyDbContext db)
    {
        db.Users.Update(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
