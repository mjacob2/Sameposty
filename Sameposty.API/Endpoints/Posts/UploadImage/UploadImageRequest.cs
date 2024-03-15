namespace Sameposty.API.Endpoints.Posts.UploadImage;

public class UploadImageRequest
{
    public int PostId { get; set; }

    public IFormFile ImageData { get; set; }

    public string OldImageUrl { get; set; }
}
