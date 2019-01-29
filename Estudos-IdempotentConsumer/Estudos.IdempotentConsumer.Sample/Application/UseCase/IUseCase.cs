namespace Estudos.IdempotentConsumer.Sample.Application.UseCase;

public interface IUseCase
{
    Task ExecuteAsync();
}

public interface IUseCase<TResult>
{
    Task<TResult> ExecuteAsync();
}