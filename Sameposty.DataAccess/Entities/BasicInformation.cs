using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;
public class BasicInformation : EntityBase
{
    [Required]
    [MaxLength(1000)]
    public string BrandName { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Audience { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Mission { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string ProductsAndServices { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Goals { get; set; } = string.Empty;

    [Required]
    [MaxLength(1000)]
    public string Assets { get; set; } = string.Empty;

    public int UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }

    public bool IsEmpty()
    {
        return string.IsNullOrEmpty(BrandName) || string.IsNullOrEmpty(Audience) || string.IsNullOrEmpty(Mission) || string.IsNullOrEmpty(ProductsAndServices) || string.IsNullOrEmpty(Goals) || string.IsNullOrEmpty(Assets);
    }
}
