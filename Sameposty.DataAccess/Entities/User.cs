using System.ComponentModel.DataAnnotations;

namespace Sameposty.DataAccess.Entities;

public class User : EntityBase
{
    [Required]
    [MaxLength(50)]
    public string Email { get; set; }

    [Required]
    [Length(8, 20)]
    public string Password { get; set; }

    public List<Post> Posts { get; set; } = [];
}
