using Microsoft.Extensions.DependencyInjection;

namespace Common.Tracing.OpenTracing;

public static class Extension
{
    public static IServiceCollection AddOpenTracingHandler(this IServiceCollection services)
    {
        services.AddTransient<InjectOpenTracingHeaderHandler>();
        return services;
    }

    public static IHttpClientBuilder WithOpenTracingHeaderHandler(this IHttpClientBuilder httpClientBuilder)
    {
        httpClientBuilder.AddHttpMessageHandler<InjectOpenTracingHeaderHandler>();
        return httpClientBuilder;
    }
}