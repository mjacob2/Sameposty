namespace Sameposty.API.Endpoints.Posts.RegenerateImage;

public class RegenerateImageRequest
{
    public int PostId { get; set; }

    public string Prompt { get; set; }

    public bool GeneratePrompt { get; set; }
}
