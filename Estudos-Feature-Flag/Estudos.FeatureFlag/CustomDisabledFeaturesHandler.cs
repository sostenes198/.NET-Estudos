using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.FeatureManagement.Mvc;

namespace Estudos.FeatureFlag
{
    public class CustomDisabledFeaturesHandler :  IDisabledFeaturesHandler
    {
        public Task HandleDisabledFeatures(IEnumerable<string> features, ActionExecutingContext context)
        {
            context.Result = new OkObjectResult($"Feature(s) Desabilitada(s): {string.Join(',', features)}");
            return Task.CompletedTask;
        }
    }
}