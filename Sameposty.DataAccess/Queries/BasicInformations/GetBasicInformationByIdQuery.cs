using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.BasicInformations;
public class GetBasicInformationByIdQuery : QueryBase<BasicInformation>
{
    public required int Id { get; set; }

    public override async Task<BasicInformation> Execute(SamepostyDbContext db)
    {
        return await db.BasicInformations.FindAsync(Id);
    }
}
