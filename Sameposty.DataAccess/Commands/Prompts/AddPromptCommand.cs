using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Prompts;
public class AddPromptCommand : CommandBase<Prompt, Prompt>
{
    public override async Task<Prompt> Execute(SamepostyDbContext db)
    {
        await db.Prompts.AddAsync(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
