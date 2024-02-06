using Sameposty.DataAccess.Commands;

namespace Sameposty.DataAccess.Executors;
public interface ICommandExecutor
{
    Task<TResoult> ExecuteCommand<TParameters, TResoult>(CommandBase<TParameters, TResoult> command);
}
