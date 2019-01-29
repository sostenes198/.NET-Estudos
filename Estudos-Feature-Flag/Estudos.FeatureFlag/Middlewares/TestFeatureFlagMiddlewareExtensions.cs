using Microsoft.AspNetCore.Builder;
using Microsoft.FeatureManagement;

namespace Estudos.FeatureFlag.Middlewares
{
    public static class TestFeatureFlagMiddlewareExtensions
    {
        public static IApplicationBuilder UseTestFeatureFlag(this IApplicationBuilder builder)
        {
            return builder.UseMiddlewareForFeature<TestFeatureFlagMiddleware>(MyFeatureFlags.FeatureMiddleware);
            // builder.UseForFeature(MyFeatureFlags.FeatureMiddleware, appBuilder =>
            // {
            //     appBuilder.UseMiddleware<TestFeatureFlagMiddleware>();
            // });
        }
    }
}