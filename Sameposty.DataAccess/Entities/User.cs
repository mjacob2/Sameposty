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

    [MaxLength(10)]
    public string NIP { get; set; } = string.Empty;

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

    public int ImageTokensLeft { get; set; }

    public int TextTokensLeft { get; set; }

    public int ImageTokensUsed { get; set; }

    public int TextTokensUsed { get; set; }

    public int PostsToGenerateLeft { get; set; }

    public int PostsGenerated { get; set; }

    public long? FakturowniaClientId { get; set; }

    public void DecreaseImageTokens()
    {
        if (ImageTokensLeft - 1 < 0)
        {
            throw new InvalidOperationException("Image tokens left cannot be less than zero.");
        }

        ImageTokensLeft--;
        ImageTokensUsed++;
    }

    public void DecreaseTextTokens()
    {
        if (TextTokensLeft - 1 < 0)
        {
            throw new InvalidOperationException("Text tokens left cannot be less than zero.");
        }

        TextTokensLeft--;
        TextTokensUsed++;
    }

    public void DecreasePostsToGenerate()
    {
        if (PostsToGenerateLeft - 1 < 0)
        {
            throw new InvalidOperationException("Posts to generate left cannot be less than zero.");
        }
        PostsToGenerateLeft--;
        PostsGenerated++;
    }
}
