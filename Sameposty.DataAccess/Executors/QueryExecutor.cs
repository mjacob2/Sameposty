using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Queries;

namespace Sameposty.DataAccess.Executors;
public class QueryExecutor(SamepostyDbContext db) : IQueryExecutor
{
    public async Task<TResult> ExecuteQuery<TResult>(QueryBase<TResult> query)
    {
        return await query.Execute(db);
    }
}
