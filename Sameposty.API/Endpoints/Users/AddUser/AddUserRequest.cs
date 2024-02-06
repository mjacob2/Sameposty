namespace Sameposty.API.Endpoints.Users.AddUser;

public class AddUserRequest
{
    public required string Email { get; set; }

    public required string Password { get; set; }
}
