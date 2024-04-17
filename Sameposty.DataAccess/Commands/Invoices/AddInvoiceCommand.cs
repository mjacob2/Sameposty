using Sameposty.DataAccess.DatabaseContext;
using Sameposty.DataAccess.Entities;

namespace Sameposty.DataAccess.Commands.Invoices;
public class AddInvoiceCommand : CommandBase<Invoice, Invoice>
{
    public override async Task<Invoice> Execute(SamepostyDbContext db)
    {
        await db.Invoices.AddAsync(Parameter);
        await db.SaveChangesAsync();
        return Parameter;
    }
}
