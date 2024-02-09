namespace Sameposty.API.Endpoints.Users.AddUser;

public class AddUserRequest
{
    public required string NIP { get; set; } = string.Empty;

    public required string Email { get; set; } = string.Empty;

    public required string Password { get; set; } = string.Empty;
}
