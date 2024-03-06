namespace Sameposty.API.Endpoints.Posts.UpdatePostDescription;

public class UpdatePostDescriptionRequest
{
    public int PostId { get; set; }

    public string PostDescription { get; set; }
}
