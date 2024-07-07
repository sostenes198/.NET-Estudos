using Hangfire.Dashboard;

namespace Scheduled.Message.Api.Hangfire.Dashboard.Authorization;

public class DashboardNoAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context)
    {
        return true;
    }
}