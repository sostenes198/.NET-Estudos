using Hangfire.Client;
using Hangfire.Common;

namespace Estudos.Hangfire._2Server;

public class FilterJobsAttribute : JobFilterAttribute, IClientFilter
{

    public void OnCreating(CreatingContext filterContext)
    {
    }

    public void OnCreated(CreatedContext filterContext)
    {
    }
}