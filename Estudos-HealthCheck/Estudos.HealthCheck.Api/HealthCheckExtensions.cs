using Estudos.HealthCheck.Api.HealthCheck;
using Estudos.HealthCheck.Api.HealthCheck.HealthCheckOptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Estudos.HealthCheck.Api
{
    public static class HealthCheckExtensions
    {
        public const string HealthCheckUiEndpoint = "/healthcheck-ui";
        public const string HealthCheckUiApiEndpoint = "/healthcheck-ui-api";
        
        public const string AllHealthCheckStatus = "/all-healthchecks-status";
        public const string AllHealthChecks = "/all-healthchecks";
        
        public const string HealthCheckReadnessStatus = "/healthcheck-readness-status";
        public const string HealthCheckReadness = "/healthcheck-readness";
        
        public const string HealthCheckLivenessStatus = "/healthcheck-liveness-status";
        public const string HealthCheckLiveness = "/healthcheck-liveness";

        private const string TagReadness = "readness";
        private const string TagLiveness = "liveness";
        private const string TagRedis = "redis";
        private const string TagSql = "Sql";
        private const string TagExampleTest = "Example";


        public static void AddApiHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecksUI(setupSettings => { setupSettings.SetEvaluationTimeInSeconds(10); })
                .AddInMemoryStorage();

            services.AddHealthChecks()
                .Add(new HealthCheckRegistration("ExampleHealthCheck", sp => sp.GetRequiredService<ExampleHealthCheck>(), HealthStatus.Unhealthy,
                    new[] {TagLiveness, TagReadness, TagExampleTest}))
                .AddRedis(configuration.GetConnectionString("CacheRedis"), name: "Redis", tags: new[] {TagReadness, TagRedis})
                .AddSqlServer(configuration.GetConnectionString("SqlServer"), name: "SqlServer", tags: new[] {TagLiveness, TagSql});
        }

        public static void UseApiHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecksUI(setup =>
            {
                setup.UIPath = HealthCheckUiEndpoint;
                setup.ApiPath = HealthCheckUiApiEndpoint;
            });
            
            app.AddAllHealthChecks();
            app.AddReadnessHealthCheck();
            app.AddLivenessHealthCheck();
        }

        private static void AddAllHealthChecks(this IApplicationBuilder app)
        {
            app.UseHealthChecks(AllHealthCheckStatus, HealthCheckOptionsStatusResponse.Create());
            app.UseHealthChecks(AllHealthChecks, HealthCheckOptionsDefaultResponse.Create());
        }

        private static void AddReadnessHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks(HealthCheckReadnessStatus, HealthCheckOptionsStatusResponse.Create(lnq => lnq.Tags.Contains(TagReadness)));
            app.UseHealthChecks(HealthCheckReadness, HealthCheckOptionsDefaultResponse.Create(lnq => lnq.Tags.Contains(TagReadness)));
        }
        
        private static void AddLivenessHealthCheck(this IApplicationBuilder app)
        {
            app.UseHealthChecks(HealthCheckLivenessStatus, HealthCheckOptionsStatusResponse.Create(lnq => lnq.Tags.Contains(TagLiveness)));
            app.UseHealthChecks(HealthCheckLiveness, HealthCheckOptionsDefaultResponse.Create(lnq => lnq.Tags.Contains(TagLiveness)));
        }


        public static void MapHealthChecks(this IEndpointRouteBuilder map)
        {
            map.MapHealthChecksUI(setup => setup.UIPath = HealthCheckUiEndpoint);
            map.MapAllHealthChecks();
            map.MapReadnessHealthCheck();
            map.MapLivenessHealthCheck();
        }

        private static void MapAllHealthChecks(this IEndpointRouteBuilder map)
        {
            map.MapHealthChecks(AllHealthCheckStatus);
            map.MapHealthChecks(AllHealthChecks);
        }

        private static void MapReadnessHealthCheck(this IEndpointRouteBuilder map)
        {
            map.MapHealthChecks(HealthCheckReadnessStatus);
            map.MapHealthChecks(HealthCheckReadness);
        }
        
        private static void MapLivenessHealthCheck(this IEndpointRouteBuilder map)
        {
            map.MapHealthChecks(HealthCheckLivenessStatus);
            map.MapHealthChecks(HealthCheckLiveness);
        }
    }
}