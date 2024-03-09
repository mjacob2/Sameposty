using FastEndpoints;
using Sameposty.DataAccess.Commands.SocialMediaConnectors;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.Services.FacebookTokenManager;

namespace Sameposty.API.Endpoints.SocialMediaConnections;

public class AddSocialMediaConnectionEndpoint(ICommandExecutor commandExecutor, IFacebookTokenManager facebookTokenManager) : Endpoint<AddSocialMediaConnectionRequest>
{
    public override void Configure()
    {
        Post("socialMediaConections");
    }

    public override async Task HandleAsync(AddSocialMediaConnectionRequest req, CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var longLivedUserToken = await facebookTokenManager.GetLongLivedUserAccessToken(req.ShortLivedUserToken);

        var longLivedPageToken = await facebookTokenManager.GetLongLivedPageAccessToken(longLivedUserToken, req.FacebookUserId, req.PageId);

        var socialMediaConnectionToAdd = new SocialMediaConnection()
        {
            UserId = userId,
            AccesToken = longLivedPageToken,
            PageId = req.PageId,
            PageName = req.PageName,
            Platform = req.Platform,
        };

        var addCommand = new AddSocialMediaConnectionCommand() { Parameter = socialMediaConnectionToAdd };

        var added = await commandExecutor.ExecuteCommand(addCommand);

        await SendOkAsync(added, ct);
    }
}
