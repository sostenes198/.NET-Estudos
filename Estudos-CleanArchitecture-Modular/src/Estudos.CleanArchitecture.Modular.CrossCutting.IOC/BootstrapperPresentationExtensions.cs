using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;

namespace Estudos.CleanArchitecture.Modular.CrossCutting.IOC;

public static class BootstrapperPresentationExtensions
{
    public static IServiceCollection AddPresenter<TOutputUseCase, TOutputPresenter>(this IServiceCollection services)
        where TOutputUseCase : class, IOutputUseCaseResult
        where TOutputPresenter : class, TOutputUseCase
    {
        services.TryAddScoped<TOutputPresenter>();
        services.TryAddScoped<TOutputUseCase>(provider => provider.GetRequiredService<TOutputPresenter>());

        return services;
    }
}