using Scheduled.Message.Application.Boundaries.UseCases.Outputs;

namespace Scheduled.Message.Application.Boundaries.UseCases;

public interface IUseCase<in TUseCaseInput, in TOutputUseCase>
    where TUseCaseInput : IUseCaseInput
    where TOutputUseCase : IUseCaseOutput
{
    Task ExecuteAsync(TUseCaseInput input, TOutputUseCase outputUseCase, CancellationToken token = default);
}