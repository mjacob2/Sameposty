
using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;
public class SocialMediaConnection
{
    public int Id { get; set; }

    public SocialMediaPlatform Platform { get; set; }

    public string AccesToken { get; set; }

    public string PageName { get; set; }

    public string PageId { get; set; }

    [JsonIgnore]
    public User User { get; set; }

    public int UserId { get; set; }
}
