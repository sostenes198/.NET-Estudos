using Estudos.CleanArchitecture.Modular.Commons.Application.UseCases.UseCaseResults;

namespace Estudos.CleanArchitecture.Modular.Commons.Application.UseCases;

public interface IUseCase<in TUseCaseInput, in TPortResult>
    where TUseCaseInput : UseCaseInput
    where TPortResult : IOutputUseCaseResult
{
    Task ExecuteAsync(TUseCaseInput input, TPortResult outputResult, CancellationToken cancellationToken = default);
}