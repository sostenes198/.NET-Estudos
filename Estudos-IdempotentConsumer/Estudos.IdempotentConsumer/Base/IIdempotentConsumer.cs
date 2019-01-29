using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Enums;

namespace Estudos.IdempotentConsumer.Base;

public interface IIdempotentConsumer<in TMessage> : IIdempotentConsumer
    where TMessage : IIdempotentConsumerKey
{
    Task<CompletionResult<TResult?>> ProcessAsync<TResult>(string instanceId, TMessage message, Func<Task<TResult?>> funcProcess);
    Task<CompletionStatus> ProcessAsync(string instanceId, TMessage message, Func<Task> funcProcess);
}

public interface IIdempotentConsumer
{
}