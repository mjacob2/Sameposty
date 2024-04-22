using System.ComponentModel.DataAnnotations;

namespace Sameposty.DataAccess.Entities;
public class Post : EntityBase
{
    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    [MaxLength(200)]
    public string ImageUrl { get; set; } = string.Empty;

    [Required]
    public DateTime ShedulePublishDate { get; set; }

    public DateTime? PublishedDate { get; set; }

    public bool IsPublished { get; set; }

    public bool IsPublishingInProgress { get; set; }

    public string JobPublishId { get; set; } = string.Empty;

    public List<PublishResult> PublishResults { get; set; }

    public int UserId { get; set; }

    public bool IsApproved { get; set; }
}
