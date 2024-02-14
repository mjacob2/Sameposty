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

    [Required]
    [MaxLength(10)]
    public string NIP { get; set; }

    /// <summary>
    /// Every new User must be onboarding. Set to false, when onboarding finish.
    /// </summary>
    [Required]
    public bool IsOnboarding { get; set; } = true;

    [MaxLength(20)]
    public string Salt { get; set; }

    public List<Post> Posts { get; set; } = [];
}
