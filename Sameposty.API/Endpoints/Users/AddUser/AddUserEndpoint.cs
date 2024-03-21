using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.EmailService;
using Sameposty.Services.Hasher;
using Sameposty.Services.JWTService;

namespace Sameposty.API.Endpoints.Users.AddUser;

public class AddUserEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor, IEmailService email, ILogger<AddUserEndpoint> logger) : Endpoint<AddUserRequest>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("addUser");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddUserRequest req, CancellationToken ct)
    {

        logger.LogWarning("I enter AddUserEndpoint");

        var getUserByEmail = new GetUserByEmailQuery() { Email = req.Email };
        var userFromDb = await queryExecutor.ExecuteQuery(getUserByEmail);

        if (userFromDb != null)
        {
            ThrowError("Nie można użyć tego adresu e-mail");
        }

        var salt = Hasher.GetSalt();
        var passwordHashed = Hasher.HashPassword(req.Password, salt);

        var user = new User()
        {
            CreatedDate = DateTime.UtcNow,
            Email = req.Email,
            Password = passwordHashed,
            Salt = salt,
            NIP = req.NIP,
        };

        var command = new AddUserCommand() { Parameter = user };

        var newUserFromDb = await commandExecutor.ExecuteCommand(command);

        string jwtToken = JWTFactory.GenerateJwt(newUserFromDb.Id.ToString());

        logger.LogWarning($"created token: {jwtToken}");

        await email.SendRegisterConfirmationEmail(req.Email, jwtToken);

        await SendOkAsync($"{user.Email}", ct);
    }
}
