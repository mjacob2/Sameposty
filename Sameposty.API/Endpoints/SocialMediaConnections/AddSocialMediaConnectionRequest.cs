using static Sameposty.DataAccess.Entities.SocialMediaConnection;

namespace Sameposty.API.Endpoints.SocialMediaConnections;

public class AddSocialMediaConnectionRequest
{
    public SocialMediaPlatform Platform { get; set; }

    public string ShortLivedUserToken { get; set; }

    public string PageName { get; set; }

    public string PageId { get; set; }

    public int UserId { get; set; }

    public string FacebookUserId { get; set; }
}
