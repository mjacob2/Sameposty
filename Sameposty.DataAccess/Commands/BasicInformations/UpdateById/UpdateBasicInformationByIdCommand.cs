using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.BasicInformations.UpdateById;
public class UpdateBasicInformationByIdCommand : CommandBase<BasicInformation, BasicInformation>
{
    public override async Task<BasicInformation> Execute(SamepostyDbContext db)
    {
        db.BasicInformations.Update(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
