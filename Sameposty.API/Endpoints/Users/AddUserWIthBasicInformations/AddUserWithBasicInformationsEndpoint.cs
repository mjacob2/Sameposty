using FastEndpoints;
using Sameposty.DataAccess.Commands.Users;
using Sameposty.DataAccess.Entities;
using Sameposty.DataAccess.Executors;
using Sameposty.DataAccess.Queries.Users;
using Sameposty.Services.Configurator;
using Sameposty.Services.EmailService;
using Sameposty.Services.Hasher;
using Sameposty.Services.JWTService;

namespace Sameposty.API.Endpoints.Users.AddUserWIthBasicInformations;

public class AddUserWithBasicInformationsEndpoint(IQueryExecutor queryExecutor, IConfigurator configurator, ICommandExecutor commandExecutor, IJWTBearerProvider jwt, IEmailService email) : Endpoint<AddUserWithBasicInformationsRequest>
{
    public override void Configure()
    {
        Verbs(Http.POST);
        Routes("addUserWithBasicInformations");
        AllowAnonymous();
    }

    public override async Task HandleAsync(AddUserWithBasicInformationsRequest req, CancellationToken ct)
    {
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
            PostsToGenerateLeft = configurator.PostsDefaultLimit,
            Role = DataAccess.Entities.Roles.FreeUser,
            BasicInformation = new BasicInformation()
            {
                BrandName = req.BrandName,
                Audience = req.Audience,
                Mission = req.Mission,
                ProductsAndServices = req.ProductsAndServices,
                Goals = req.Goals,
                Assets = req.Assets
            }
        };

        var command = new AddUserCommand() { Parameter = user };

        var newUserFromDb = await commandExecutor.ExecuteCommand(command);

        var newToken = jwt.ProvideToken(newUserFromDb.Id.ToString(), newUserFromDb.Email, newUserFromDb.Role.ToString());

        await email.SendRegisterConfirmationEmail(req.Email, newToken);

        await SendOkAsync($"{user.Email}", ct);
    }
}
