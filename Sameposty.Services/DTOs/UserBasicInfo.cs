namespace Sameposty.Services.DTOs;

public class UserBasicInfo
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Nip { get; set; }
    public string Name { get; set; }
    public string City { get; set; }
    public string Role { get; set; }
    public bool IsVerified { get; set; }
    public int ImageTokensLeft { get; set; }
    public int TextTokensLeft { get; set; }
    public long? FakturowniaClientId { get; set; }
    public bool HasSubscription { get; set; }
}