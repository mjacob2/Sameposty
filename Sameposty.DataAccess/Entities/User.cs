using System.ComponentModel.DataAnnotations;

namespace Sameposty.DataAccess.Entities;

public class User : EntityBase
{
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }

    [Required]
    [MaxLength(100)]
    public string Password { get; set; }

    [MaxLength(20)]
    public string Salt { get; set; }

    [Required]
    [MaxLength(10)]
    public string NIP { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string BuildingNumber { get; set; } = string.Empty;

    public string FlatNumber { get; set; } = string.Empty;

    public string PostCode { get; set; } = string.Empty;

    public string REGON { get; set; } = string.Empty;

    /// <summary>
    /// Information from REGON API about current status of the company
    /// </summary>
    public bool IsSuspended { get; set; }

    /// <summary>
    /// Information from REGON API about current status of the company
    /// </summary>
    public bool IsActive { get; set; }

    public List<Post> Posts { get; set; } = [];

    public List<Invoice> Invoices { get; set; } = [];

    public BasicInformation BasicInformation { get; set; } = new();

    public FacebookConnection? FacebookConnection { get; set; }

    public InstagramConnection? InstagramConnection { get; set; }

    public Subscription Subscription { get; set; } = new();

    public Privilege Privilege { get; set; } = new();

    public Roles Role { get; set; } = Roles.FreeUser;

    public bool IsVerified { get; set; }

    public int ImageTokensLimit { get; set; }

    public int ImageTokensUsed { get; set; }

    public int TextTokensLimit { get; set; }

    public int TextTokensUsed { get; set; }

    public long? FakturowniaClientId { get; set; }

    public int GetImageTokensLeft()
    {
        return ImageTokensLimit - ImageTokensUsed;
    }

    public int GetTextTokensLeft()
    {
        return TextTokensLimit - TextTokensUsed;
    }
}
