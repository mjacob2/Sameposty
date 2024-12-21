using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.EmailManager;
using Sameposty.Services.JWTService;

namespace Sameposty.API.Endpoints.Users.ResetPassword;

public class ResetPasswordEndpoint(IQueryExecutor queryExecutor, IEmailService email, IJWTBearerProvider jwt) : Endpoint<ResetPasswordRequest>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("reset-password");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
    {
        var getUserByEmail = new GetUserByEmailQuery(req.Email);

        var userFromDb = await queryExecutor.ExecuteQuery(getUserByEmail);

        if (userFromDb != null)
        {
            string token = jwt.ProvideToken(userFromDb.Id.ToString(), userFromDb.Email, userFromDb.Role.ToString());
            await email.SendResetPasswordEmail(req.Email, token, userFromDb.Id);
        }

        await SendOkAsync($"Request received. The rest is mistery...", ct);
    }
}
