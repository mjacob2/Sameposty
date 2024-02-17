using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;
public class Post : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; }

    [Required]
    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public DateTime PublishDate { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }

    public int UserId { get; set; }
}
