namespace Sameposty.API.Endpoints.InstagramConnections.Add;

public class AddInstagramConnectionRequest
{
    public string ShortLivedUserToken { get; set; }

    public string PageName { get; set; }

    public string PageId { get; set; }

    public int UserId { get; set; }

    public string InstagramUserId { get; set; }
}
