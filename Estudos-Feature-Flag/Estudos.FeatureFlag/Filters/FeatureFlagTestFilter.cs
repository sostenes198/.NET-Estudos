using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Estudos.FeatureFlag.Filters
{
    public class FeatureFlagTestFilter : IAsyncActionFilter
    {
        public Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.Result = new OkObjectResult($"Feature Flag {MyFeatureFlags.FeatureFilter} habilitada");
            return Task.CompletedTask;
        }
    }
}