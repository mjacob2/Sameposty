using Sameposty.DataAccess.DatabaseContext;

namespace Sameposty.DataAccess.Commands;
public abstract class CommandBase<TParameter, TResoult>
{
    public required TParameter Parameter { get; set; }

    public abstract Task<TResoult> Execute(SamepostyDbContext db);
}
