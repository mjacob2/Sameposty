using Sameposty.DataAccess.Queries;

namespace Sameposty.DataAccess.Executors;
public interface IQueryExecutor
{
    Task<TResult> ExecuteQuery<TResult>(QueryBase<TResult> query);
}
