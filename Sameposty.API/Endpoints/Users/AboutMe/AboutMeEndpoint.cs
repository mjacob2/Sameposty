using FastEndpoints;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;

namespace Sameposty.API.Endpoints.Users.AboutMe;

public class AboutMeEndpoint(IQueryExecutor queryExecutor) : EndpointWithoutRequest<AboutMeResponse>
{
    public override void Configure()
    {
        Get("about-me");
    }
    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = User.FindFirst("UserId").Value;
        var loggedUserId = int.Parse(id);

        var userFromDb = await queryExecutor.ExecuteQuery(new GetUserAboutMeQuery(loggedUserId));

        var resposne = new AboutMeResponse()
        {
            Id = userFromDb.Id,
            Email = userFromDb.Email,
            NIP = userFromDb.NIP,
            Role = GetRoleString(userFromDb.Role),
            IsVerified = userFromDb.IsVerified,
            CanGenerateInitialPosts = userFromDb.Privilege.CanGenerateInitialPosts,
            CanEditImageAI = userFromDb.Privilege.CanEditImageAI,
            CanGenerateImageAI = userFromDb.Privilege.CanGenerateImageAI,
            CanGenerateTextAI = userFromDb.Privilege.CanGenerateTextAI,
            ImageTokensLimit = userFromDb.ImageTokensLimit,
            ImageTokensUsed = userFromDb.ImageTokensUsed,
            TextTokensLimit = userFromDb.TextTokensLimit,
            TextTokensUsed = userFromDb.TextTokensUsed,
        };

        await SendOkAsync(resposne, ct);
    }

    private static string GetRoleString(Roles role)
    {
        return role switch
        {
            DataAccess.Entities.Roles.FreeUser => "Konto bezpłatne",
            DataAccess.Entities.Roles.PaidUser => "Konto płatne",
            DataAccess.Entities.Roles.Admin => "Administrator",
            _ => throw new ArgumentException("Invalid role"),
        };
    }
}
