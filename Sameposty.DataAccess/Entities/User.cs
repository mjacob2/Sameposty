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

    public string CompanyDescription { get; set; } = string.Empty;

    public List<Post> Posts { get; set; } = [];

    public BasicInformation BasicInformation { get; set; } = new();

    public FacebookConnection? FacebookConnection { get; set; }

    public InstagramConnection? InstagramConnection { get; set; }

    public Privilege Privilege { get; set; } = new();

    public Roles Role { get; set; } = Roles.FreeUser;

    public bool IsVerified { get; set; }
}
