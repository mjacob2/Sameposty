using Sameposty.DataAccess.Entities;

namespace Sameposty.Services.PostsPublishers;
public class ConnectionsModel
{
    public FacebookConnection? FacebookConnection { get; set; }

    public InstagramConnection? InstagramConnection { get; set; }
}
