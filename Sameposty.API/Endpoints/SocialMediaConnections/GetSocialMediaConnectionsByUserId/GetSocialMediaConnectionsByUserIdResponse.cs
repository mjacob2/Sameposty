using Sameposty.DataAccess.Entities;

namespace Sameposty.API.Endpoints.SocialMediaConnections.GetSocialMediaConnectionsByUserId;

public class GetSocialMediaConnectionsByUserIdResponse(SocialMediaConnection socialMediaConnection)
{
    public int Id { get; set; } = socialMediaConnection.Id;

    public string Platform { get; set; } = socialMediaConnection.Platform.ToString();

    public string PageName { get; set; } = socialMediaConnection.PageName;

    public string PageId { get; set; } = socialMediaConnection.PageId;

    public bool HasValidAccessToken { get; set; } = !string.IsNullOrEmpty(socialMediaConnection.AccesToken);
}
