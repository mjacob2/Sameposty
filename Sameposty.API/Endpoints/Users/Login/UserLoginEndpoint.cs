using FastEndpoints;
using Microsoft.AspNetCore.Identity.Data;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Hasher;
using Sameposty.Services.JWTService;
using Sameposty.Services.Secrets;

namespace Sameposty.API.Endpoints.Users.Login;

public class UserLoginEndpoint(IQueryExecutor queryExecutor, IJWTBearerProvider jwt, ISecretsProvider secrets) : Endpoint<LoginRequest>
{
    private const string AdminName = "admin";
    private const string AdminId = "0";

    public override void Configure()
    {
        Post("login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        if (IsAdmin(secrets, req))
        {
            var admintoken = jwt.ProvideToken(AdminId, AdminName, DataAccess.Entities.Roles.Admin.ToString());

            await SendAsync(new
            {
                Id = AdminId,
                Token = admintoken,
                Username = AdminName,
            }, cancellation: ct);
        }
        else
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

            var passwordFromRequest = Hasher.HashPassword(req.Password, userFromDb.Salt);
            var passwordFromDb = userFromDb.Password;

            if (passwordFromDb != passwordFromRequest)
            {
                ThrowError("Niepoprawne hasło");
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

    private static bool IsAdmin(ISecretsProvider secrets, LoginRequest req)
    {
        return req.Email == "admin" && req.Password == secrets.AdminPassword;
    }
}
