using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;
public class Privilege : EntityBase
{
    public bool CanGenerateImageAI { get; set; } = true;

    public bool CanEditImageAI { get; set; } = true;

    public bool CanGenerateTextAI { get; set; } = true;

    public int UserId { get; set; }

    [JsonIgnore]
    public User User { get; set; }
}
