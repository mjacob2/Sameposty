namespace Sameposty.API.Endpoints.FacebookConnections.Add;

public class AddSocialMediaConnectionRequest
{
    public string ShortLivedUserToken { get; set; }

    public string PageName { get; set; }

    public string PageId { get; set; }

    public int UserId { get; set; }

    public string FacebookUserId { get; set; }
}
