using FastEndpoints;
using FastEndpoints.Security;

namespace Sameposty.API.Endpoints.Users.Login;
public class MyTokenService : RefreshTokenService<TokenRequest, TokenResponse>
{
    public MyTokenService(IConfiguration config)
    {
        Setup(o =>
        {
            o.TokenSigningKey = config["TokenSigningKey"];
            o.AccessTokenValidity = TimeSpan.FromMinutes(5);
            o.RefreshTokenValidity = TimeSpan.FromHours(4);

            o.Endpoint("/api/refresh-token", ep =>
            {
                ep.Summary(s => s.Summary = "this is the refresh token endpoint");
            });
        });
    }

    public override Task PersistTokenAsync(TokenResponse response)
    {
        throw new NotImplementedException();
        // this method will be called whenever a new access/refresh token pair is being generated.
        // store the tokens and expiry dates however you wish for the purpose of verifying
        // future refresh requests. 
    }

    public override Task RefreshRequestValidationAsync(TokenRequest req)
    {
        throw new NotImplementedException();
        // validate the incoming refresh request by checking the token and expiry against the
        // previously stored data. if the token is not valid and a new token pair should
        // not be created, simply add validation errors using the AddError() method.
        // the failures you add will be sent to the requesting client. if no failures are added,
        // validation passes and a new token pair will be created and sent to the client.  
    }

    public override Task SetRenewalPrivilegesAsync(TokenRequest request, UserPrivileges privileges)
    {
        throw new NotImplementedException();
        // specify the user privileges to be embedded in the jwt when a refresh request is
        // received and validation has passed. this only applies to renewal/refresh requests
        // received to the refresh endpoint and not the initial jwt creation. 
    }
}