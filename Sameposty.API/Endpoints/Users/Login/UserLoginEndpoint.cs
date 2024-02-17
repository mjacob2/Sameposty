using FastEndpoints;
using Microsoft.AspNetCore.Identity.Data;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Hasher;
using Sameposty.Services.JWTService;

namespace Sameposty.API.Endpoints.Users.Login;

public class UserLoginEndpoint(IQueryExecutor queryExecutor) : Endpoint<LoginRequest>
{
    public override void Configure()
    {
        Post("login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var getUserByEmail = new GetUserByEmailQuery() { Email = req.Email };
        var userFromDb = await queryExecutor.ExecuteQuery(getUserByEmail);

        if (userFromDb == null)
        {
            ThrowError("Email nie istnieje");
        }

        var passwordFromRequest = Hasher.HashPassword(req.Password, userFromDb.Salt);
        var passwordFromResponse = userFromDb.Password;

        if (passwordFromResponse != passwordFromRequest)
        {
            ThrowError("Niepoprawne hasło");
        }

        string jwtToken = JWTFactory.GenerateJwt(userFromDb.Id.ToString());

        await SendAsync(new
        {
            Id = userFromDb.Id,
            Token = jwtToken,
            Username = req.Email,
        }, cancellation: ct);

    }
}
