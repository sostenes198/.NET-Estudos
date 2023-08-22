using Microsoft.Extensions.DependencyInjection;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;
using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;

namespace Estudos.CleanArchitecture.Modular.Infrastructure.UseCases;

internal sealed class UseCaseManager : IUseCaseManager
{
    private readonly IServiceProvider _serviceProvider;

    public UseCaseManager(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public Task ExecuteAsync<TInput, TPortResult>(TInput input, TPortResult outputPortResult, CancellationToken cancellationToken = default)
        where TInput : UseCaseInput
        where TPortResult : IOutputUseCaseResult
    {
        var useCase = _serviceProvider.GetRequiredService<IUseCase<TInput, TPortResult>>();

        return useCase.ExecuteAsync(input, outputPortResult, cancellationToken);
    }
}