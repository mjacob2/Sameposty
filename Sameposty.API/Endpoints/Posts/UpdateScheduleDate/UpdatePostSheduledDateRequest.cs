namespace Sameposty.API.Endpoints.Posts.UpdateScheduleDate;

public class UpdatePostSheduledDateRequest
{
    public int PostId { get; set; }

    public DateTime Date { get; set; }
}
