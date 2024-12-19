namespace Sameposty.API.Endpoints.Posts.AddPost;

public class AddPostRequest
{
    public DateTime Date { get; set; }

    public bool GenerateImage { get; set; }

    public bool GenerateText { get; set; }
}
