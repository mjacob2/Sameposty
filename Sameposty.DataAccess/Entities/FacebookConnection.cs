
using System.Text.Json.Serialization;

namespace Sameposty.DataAccess.Entities;

public class FacebookConnection : EntityBase
{
    public string AccesToken { get; set; } = string.Empty;

    public string PageName { get; set; } = string.Empty;

    public string PageId { get; set; } = string.Empty;

    [JsonIgnore]
    public User User { get; set; }

    public int UserId { get; set; }
}
