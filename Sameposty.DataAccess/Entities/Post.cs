using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;
public class Post : EntityBase
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    public DateTime ShedulePublishDate { get; set; }

    public DateTime? PublishedDate { get; set; }

    public string PlatformPostId { get; set; } = string.Empty;

    public bool IsPublished { get; set; }

    [JsonIgnore]
    public virtual User User { get; set; }

    public int UserId { get; set; }
}
