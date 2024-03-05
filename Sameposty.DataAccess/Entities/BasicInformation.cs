using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;
public class BasicInformation : EntityBase
{
    [Required]
    [MaxLength(200)]
    public string Branch { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string ProductsAndServices { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Goals { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Assets { get; set; } = string.Empty;

    public int UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }
}
