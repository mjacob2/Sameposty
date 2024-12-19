using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;
using Sameposty.Services.FacebookPixel;
using Sameposty.Services.Hasher;
using Sameposty.Services.JWTService;

namespace Sameposty.API.Endpoints.Users.AddUser;

public class AddUserEndpoint(ICommandExecutor commandExecutor, IQueryExecutor queryExecutor, IEmailService email, IConfigurator configurator, IJWTBearerProvider jwt, IFacebookPixelNotifier pixelNotifier) : Endpoint<AddUserRequest>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("addUser");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddUserRequest req, CancellationToken ct)
    {
        //if (req.Email.Contains('+'))
        //{
        //    ThrowError("Nie używaj znaku + w adresie e-mail");
        //}

        var getUserByEmail = new GetUserByEmailQuery(req.Email);
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
            ImageTokensLeft = configurator.ImageTokensDefaultLimit,
            TextTokensLeft = configurator.TextTokensDefaultLimit,
            Role = DataAccess.Entities.Roles.FreeUser,
            PostsToGenerateLeft = configurator.PostsDefaultLimit,
        };

        var command = new AddUserCommand() { Parameter = user };

        var newUserFromDb = await commandExecutor.ExecuteCommand(command);

        var newToken = jwt.ProvideToken(newUserFromDb.Id.ToString(), newUserFromDb.Email, newUserFromDb.Role.ToString());

        await email.SendRegisterConfirmationEmail(req.Email, newToken);

        await pixelNotifier.NotifyNewPurchaseAsync(req.Email);

        await SendOkAsync($"{user.Email}", ct);
    }
}
