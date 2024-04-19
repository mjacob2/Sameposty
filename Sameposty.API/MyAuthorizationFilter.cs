using Hangfire.Dashboard;

namespace Sameposty.API;

public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{

    public bool Authorize(DashboardContext context)
    {
        //var loggedUserId = User.FindFirst("UserId").Value;

        var httpContext = context.GetHttpContext();

        // Only allow users with the "Admin" role to see the Dashboard
        return httpContext.User.IsInRole("Manager");
    }


}
