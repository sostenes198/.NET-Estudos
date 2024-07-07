using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Scheduled.Message.Application.Boundaries.UseCases.Outputs;

namespace Scheduled.Message.Api.Bootstrappers;

[ExcludeFromCodeCoverage]
public static class Bootstrapper
{
    public static IServiceCollection BootstrapperApplication(this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        services
            .InitializeApi()
            .InitializeApplication()
            .InitializeInfrastructure(configuration);

        return services;
    }

    public static IServiceCollection AddPresenter<TOutputUseCase, TOutputPresenter>(this IServiceCollection services)
        where TOutputUseCase : class, IUseCaseOutput
        where TOutputPresenter : class, TOutputUseCase
    {
        services.TryAddScoped<TOutputPresenter>();
        services.TryAddScoped<TOutputUseCase>(provider => provider.GetRequiredService<TOutputPresenter>());

        return services;
    }
}