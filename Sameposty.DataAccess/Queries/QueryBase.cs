using Sameposty.DataAccess.DatabaseContext;

namespace Sameposty.DataAccess.Queries;
public abstract class QueryBase<TResult>
{
    public abstract Task<TResult> Execute(SamepostyDbContext db);
}
