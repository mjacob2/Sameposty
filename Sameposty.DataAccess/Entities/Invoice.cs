namespace Sameposty.DataAccess.Entities;
public class Invoice : EntityBase
{
    public string Number { get; set; } = string.Empty;

    public string IssueDate { get; set; } = string.Empty;

    public string PriceNet { get; set; } = string.Empty;

    public string PriceGross { get; set; } = string.Empty;

    public string Currency { get; set; } = string.Empty;

    public long FakturowniaClientId { get; set; }

    public long FakturowniaInvoiceId { get; set; }

    public int UserId { get; set; }
}
