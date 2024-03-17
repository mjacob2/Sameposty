using Hangfire.Dashboard;

namespace Sameposty.API;

public class MyAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        // Allow all users, including unauthenticated ones, to see the Dashboard.
        return true;
    }
}
