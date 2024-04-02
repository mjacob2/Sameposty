namespace Sameposty.API.Endpoints.Users.AboutMe;

public class AboutMeResponse
{
    public int Id { get; set; }
    public string Email { get; set; }

    public string NIP { get; set; }

    public string Role { get; set; }

    public bool IsVerified { get; set; }

    public bool CanGenerateInitialPosts { get; set; }

    public bool CanGenerateImageAI { get; set; }

    public bool CanEditImageAI { get; set; }

    public bool CanGenerateTextAI { get; set; }

    public int ImageTokensLimit { get; set; }

    public int ImageTokensUsed { get; set; }

    public int TextTokensLimit { get; set; }

    public int TextTokensUsed { get; set; }
}
