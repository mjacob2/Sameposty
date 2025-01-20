namespace Sameposty.DataAccess.Queries;

public interface IQuery<TContext, TResult>
{
    Task<TResult> Execute(TContext db);
}