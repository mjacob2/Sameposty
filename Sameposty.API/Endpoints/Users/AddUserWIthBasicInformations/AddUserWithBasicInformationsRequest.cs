using Sameposty.API.Endpoints.Users.AddUser;

namespace Sameposty.API.Endpoints.Users.AddUserWIthBasicInformations;

public class AddUserWithBasicInformationsRequest : AddUserRequest
{
    public string BrandName { get; set; } = string.Empty;

    public string Audience { get; set; } = string.Empty;

    public string Mission { get; set; } = string.Empty;

    public string ProductsAndServices { get; set; } = string.Empty;

    public string Goals { get; set; } = string.Empty;

    public string Assets { get; set; } = string.Empty;
}
