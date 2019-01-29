using Estudos.IdempotentConsumer.Sample.Application.UseCase;

namespace Estudos.IdempotentConsumer.Sample.Application;

public class UseCaseWithoutReturn : IUseCase
{
    public Task ExecuteAsync()
    {
        return Task.CompletedTask;
    }
}