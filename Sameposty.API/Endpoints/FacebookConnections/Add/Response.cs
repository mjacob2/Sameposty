namespace Sameposty.API.Endpoints.FacebookConnections.Add;

public class Response
{
    public int Id { get; set; }

    public string PageName { get; set; }

    public string PageId { get; set; }

    public bool HasValidAccesToken { get; set; }
}
