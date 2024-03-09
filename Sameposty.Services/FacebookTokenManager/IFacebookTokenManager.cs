namespace Sameposty.Services.FacebookTokenManager;
public interface IFacebookTokenManager
{
    Task<string> GetLongLivedUserAccessToken(string shortLivedUserAccessToken);

    Task<string> GetLongLivedPageAccessToken(string longLivedUserAccessToken, string facebookUserId, string pageId);
}
