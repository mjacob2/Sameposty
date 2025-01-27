namespace Sameposty.API.Endpoints.Users.UpdateUser;

public class UpdateUserRequest
{
    public string Name { get; set; }

    public string NIP { get; set; }

    public string Street { get; set; }

    public string PostCode { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

}
