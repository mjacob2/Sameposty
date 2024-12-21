using FastEndpoints.Security;
using Sameposty.Services.SecretsService;

namespace Sameposty.Services.JWTService;
public class JWTBearerProvider(ISecretsProvider secrets) : IJWTBearerProvider
{
    public string ProvideToken(string id, string username, string role)
    {
        var jwtToken = JwtBearer.CreateToken(
                o =>
                {
                    o.SigningKey = secrets.JWTBearerTokenSignKey;
                    o.ExpireAt = DateTime.UtcNow.AddDays(10);
                    o.User.Roles.Add(role);
                    o.User.Claims.Add(("Username", username));
                    o.User["UserId"] = id;
                });

        return jwtToken;
    }
}
