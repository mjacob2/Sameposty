using Sameposty.DataAccess.Commands;
using Sameposty.DataAccess.DatabaseContext;

namespace Sameposty.DataAccess.Executors;
public class CommandExecutor(SamepostyDbContext db) : ICommandExecutor
{
    public Task<TResult> ExecuteCommand<TParameters, TResult>(CommandBase<TParameters, TResult> command)
    {
        return command.Execute(db);
    }
}
