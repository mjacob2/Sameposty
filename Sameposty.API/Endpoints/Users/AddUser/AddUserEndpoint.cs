using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Hasher;
using Sameposty.Services.JWTService;

namespace Sameposty.API.Endpoints.Users.AddUser;

public class AddUserEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor) : Endpoint<AddUserRequest, AddUserResponse>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("addUser");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddUserRequest req, CancellationToken ct)
    {
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

        var response = new AddUserResponse()
        {
            Id = newUserFromDb.Id,
            Token = jwtToken,
            Username = newUserFromDb.Email,
        };

        await SendOkAsync(response, ct);
    }
}
