using Scheduled.Message.Application.Boundaries.UseCases.Outputs;

namespace Scheduled.Message.Application.Boundaries.UseCases;

public interface IUseCaseManager
{
    Task ExecuteAsync<TInput, TOutput>(TInput input, TOutput output, CancellationToken token = default)
        where TInput : IUseCaseInput
        where TOutput : IUseCaseOutput;
}