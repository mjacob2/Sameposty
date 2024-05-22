using FastEndpoints;
using Sameposty.DataAccess.Commands.FacebookConnections;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.FacebookConnections;
using Sameposty.Services.FacebookTokenManager;

namespace Sameposty.API.Endpoints.FacebookConnections.Add;

public class AddFacebookConnectionEndpoint(ICommandExecutor commandExecutor, IFacebookTokenManager facebookTokenManager, IQueryExecutor queryExecutor) : Endpoint<AddSocialMediaConnectionRequest, Response>
{
    public override void Configure()
    {
        Post("facebookConnection");
    }

    public override async Task HandleAsync(AddSocialMediaConnectionRequest req, CancellationToken ct)
    {
        var facebookConnection = await queryExecutor.ExecuteQuery(new GetFacebookConnectionByPageIdQuery(req.PageId));

        if (facebookConnection != null)
        {
            ThrowError("Ta strona została już dodana na innym koncie, innego użytkownika");
        }

        var loggedUserId = User.FindFirst("UserId").Value;
        var userId = int.Parse(loggedUserId);

        var longLivedUserToken = await facebookTokenManager.GetLongLivedUserAccessToken(req.ShortLivedUserToken);

        var longLivedPageToken = await facebookTokenManager.GetLongLivedPageAccessToken(longLivedUserToken, req.FacebookUserId, req.PageId);

        var facebookConnectionToAdd = new FacebookConnection()
        {
            AccesToken = longLivedPageToken,
            PageId = req.PageId,
            PageName = req.PageName,
            UserId = userId,
        };

        var newFacebookConnection = await commandExecutor.ExecuteCommand(new AddFacebookConnectionCommand() { Parameter = facebookConnectionToAdd });

        var response = new Response()
        {
            HasValidAccesToken = true,
            Id = newFacebookConnection.Id,
            PageId = newFacebookConnection.PageId,
            PageName = newFacebookConnection.PageName,
        };

        await SendOkAsync(response, ct);
    }
}
