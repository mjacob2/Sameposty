namespace Sameposty.Services.FacebookTokenManagerService;
public interface IFacebookTokenManager
{
    Task<string> GetLongLivedUserAccessToken(string shortLivedUserAccessToken);

    Task<string> GetLongLivedPageAccessToken(string longLivedUserAccessToken, string facebookUserId, string pageId);
}
