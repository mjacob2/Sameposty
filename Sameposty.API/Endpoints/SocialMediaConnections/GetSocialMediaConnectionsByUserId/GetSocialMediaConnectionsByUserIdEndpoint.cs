using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.SocialMediaConnections;

namespace Sameposty.API.Endpoints.SocialMediaConnections.GetSocialMediaConnectionsByUserId;

public class GetSocialMediaConnectionsByUserIdEndpoint(IQueryExecutor queryExecutor) : EndpointWithoutRequest
{
    public override void Configure()
    {
        Verbs(Http.GET);
        Routes("socialMediaConnectionsByUserId");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var loggedUserId = User.FindFirst("UserId").Value;

        var loggedUserIdInt = int.Parse(loggedUserId);

        var query = new GetSocialMediaConnectionsByUserIdQuery(loggedUserIdInt);

        var socialMediaConnections = await queryExecutor.ExecuteQuery(query);

        var response = new List<GetSocialMediaConnectionsByUserIdResponse>();

        foreach (var socialMediaConnection in socialMediaConnections)
        {
            var responseItem = new GetSocialMediaConnectionsByUserIdResponse(socialMediaConnection);

            response.Add(responseItem);
        }

        await SendOkAsync(response, ct);
    }

}
