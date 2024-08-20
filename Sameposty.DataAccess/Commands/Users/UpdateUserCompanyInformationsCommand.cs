using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Users;
public class UpdateUserCompanyInformationsCommand(int userId, string newNip, string newName, string newCity, string newPostcode, string newStreet, string newBuildingNumber, string newFlatNumber, string newRegon) : CommandBase<User, User>
{
    public override async Task<User> Execute(SamepostyDbContext db)
    {
        var user = await db.Users.FindAsync(userId) ?? throw new Exception($"id: {userId}. Nie ma takiego użytkownika");

        user.NIP = newNip;
        user.Name = newName;
        user.City = newCity;
        user.PostCode = newPostcode;
        user.Street = newStreet;
        user.BuildingNumber = newBuildingNumber;
        user.FlatNumber = newFlatNumber;
        user.REGON = newRegon;


        db.Users.Update(user);
        await db.SaveChangesAsync();
        return user;
    }
}
