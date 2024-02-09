using FastEndpoints.Security;

namespace Sameposty.Services.JWTService;
public static class JWTFactory
{
    private static readonly string[] collection = ["Admin", "Manager"];
    private static readonly string[] collection0 = ["ManageUsers", "ManageInventory"];

    public static string GenerateJwt()
    {
        return JWTBearer.CreateToken(
            signingKey: "hfgrtgdhgjhtudfghtyrewnhfjejwhufgdhgufghidgjid",
            expireAt: DateTime.UtcNow.AddDays(1),
            privileges: u =>
            {
                u.Roles.Add("Manager");
                u.Permissions.AddRange(collection0);
                u.Claims.Add(new("UserName", "ko"));
                u["UserID"] = "001";
            });
    }
}
