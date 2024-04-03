namespace Sameposty.API.Endpoints.Posts.UpdateIsApproved;

public class UpdateIsApprovedRequest
{
    public int PostId { get; set; }

    public bool IsApproved { get; set; }
}
