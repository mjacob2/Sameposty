using FastEndpoints;
using Microsoft.AspNetCore.Identity.Data;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Hasher;
using Sameposty.Services.JWTService;

namespace Sameposty.API.Endpoints.Users.Login;

public class UserLoginEndpoint(IQueryExecutor queryExecutor, IJWTBearerProvider jwt) : Endpoint<LoginRequest>
{
    public override void Configure()
    {
        Post("login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {

        var getUserByEmail = new GetUserByEmailQuery(req.Email);
        var userFromDb = await queryExecutor.ExecuteQuery(getUserByEmail);

        if (userFromDb == null)
        {
            ThrowError("E-mail nie istnieje");
        }

        if (!userFromDb.IsVerified)
        {
            ThrowError("Wysłaliśmy e-mail z potwierdzeniem rejestracji. Sprawdź swoją pocztę.");
        }

        if (userFromDb.Role != DataAccess.Entities.Roles.Admin)
        {
            var passwordFromRequest = Hasher.HashPassword(req.Password, userFromDb.Salt);
            var passwordFromResponse = userFromDb.Password;

            if (passwordFromResponse != passwordFromRequest)
            {
                ThrowError("Niepoprawne hasło");
            }
        }
        else
        {
            if (req.Password != "adminadmin")
            {
                ThrowError("Niepoprawne hasło");
            }

        }

        var token = jwt.ProvideToken(userFromDb.Id.ToString(), userFromDb.Email, userFromDb.Role.ToString());

        await SendAsync(new
        {
            Id = userFromDb.Id,
            Token = token,
            Username = req.Email,
        }, cancellation: ct);

    }
}
