using System.ComponentModel.DataAnnotations;

namespace Sameposty.API.Endpoints.Posts.UpdatePostDescription;

public class UpdatePostDescriptionRequest
{
    public int PostId { get; set; }

    [MaxLength(10)]
    public string PostDescription { get; set; }
}
