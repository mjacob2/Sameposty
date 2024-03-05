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

    public List<SocialMediaConnection> SocialMediaConnections { get; set; } = [];

    public BasicInformation BasicInformation { get; set; }
}
