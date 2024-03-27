using FastEndpoints;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.EmailService;
using Sameposty.Services.JWTService;

namespace Sameposty.API.Endpoints.Users.ResetPassword;

public class ResetPasswordEndpoint(IQueryExecutor queryExecutor, IEmailService email) : Endpoint<ResetPasswordRequest>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("reset-password");
        AllowAnonymous();
    }

    public override async Task HandleAsync(ResetPasswordRequest req, CancellationToken ct)
    {
        var getUserByEmail = new GetUserByEmailQuery() { Email = req.Email };

        var userFromDb = await queryExecutor.ExecuteQuery(getUserByEmail);

        if (userFromDb != null)
        {
            string jwtToken = JWTFactory.GenerateJwt(userFromDb.Id.ToString());
            await email.SendResetPasswordEmail(req.Email, jwtToken, userFromDb.Id);
        }

        await SendOkAsync($"Request received. The rest is mistery...", ct);
    }
}
