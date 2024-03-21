using Sameposty.DataAccess.Entities;

namespace Sameposty.API.Endpoints.Users.GetUserConenctions;

public class Response
{
    public Response(User user)
    {
        if (user.FacebookConnection != null)
        {
            FacebookConnection = new FacebookConnectionModel()
            {
                HasValidAccesToken = !string.IsNullOrEmpty(user.FacebookConnection.AccesToken),
                Id = user.FacebookConnection.Id,
                PageId = user.FacebookConnection.PageId,
                PageName = user.FacebookConnection.PageName,
            };
        }

        if (user.InstagramConnection != null)
        {
            InstagramConnection = new InstagramConnectionModel()
            {
                HasValidAccesToken = !string.IsNullOrEmpty(user.InstagramConnection.AccesToken),
                Id = user.InstagramConnection.Id,
                PageId = user.InstagramConnection.PageId,
                PageName = user.InstagramConnection.PageName,
            };
        }


    }
    public FacebookConnectionModel FacebookConnection { get; set; }

    public InstagramConnectionModel InstagramConnection { get; set; }
}

public class InstagramConnectionModel
{
    public int Id { get; set; }

    public string PageName { get; set; }

    public string PageId { get; set; }

    public bool HasValidAccesToken { get; set; }
}

public class FacebookConnectionModel
{
    public int Id { get; set; }

    public string PageName { get; set; }

    public string PageId { get; set; }

    public bool HasValidAccesToken { get; set; }
}