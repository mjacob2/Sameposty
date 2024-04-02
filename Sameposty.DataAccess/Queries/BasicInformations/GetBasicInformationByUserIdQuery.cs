using Microsoft.EntityFrameworkCore;
using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Queries.BasicInformations;
public class GetBasicInformationByUserIdQuery(int userId) : QueryBase<BasicInformation>
{
    public override async Task<BasicInformation> Execute(SamepostyDbContext db)
    {
        return await db.BasicInformations.FirstOrDefaultAsync(x => x.UserId == userId);
    }
}
