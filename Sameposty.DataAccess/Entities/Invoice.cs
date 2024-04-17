namespace Sameposty.DataAccess.Entities;
public class Invoice : EntityBase
{
    public string Number { get; set; }

    public string IssueDate { get; set; }

    public string PriceNet { get; set; }

    public string PriceGross { get; set; }

    public string Currency { get; set; }

    public long FakturowniaClientId { get; set; }

    public int UserId { get; set; }
}
