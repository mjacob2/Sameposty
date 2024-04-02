namespace Sameposty.API.Endpoints.Posts.RegeneratePostDescription;

public class RegeneratePostDescriptionRequest
{
    public int PostId { get; set; }

    public string Prompt { get; set; }
}
