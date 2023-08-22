using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;

namespace Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;

public interface IUseCaseManager
{
    Task ExecuteAsync<TInput, TPortResult>(TInput input, TPortResult outputPortResult, CancellationToken cancellationToken = default)
        where TInput : UseCaseInput
        where TPortResult : IOutputUseCaseResult;
}