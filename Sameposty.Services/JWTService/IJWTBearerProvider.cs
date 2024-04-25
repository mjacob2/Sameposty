namespace Sameposty.Services.JWTService;
public interface IJWTBearerProvider
{
    string ProvideToken(string id, string username, string role);
}
