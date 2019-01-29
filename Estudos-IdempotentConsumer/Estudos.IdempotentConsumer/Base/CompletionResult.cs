using Estudos.IdempotentConsumer.Enums;

namespace Estudos.IdempotentConsumer.Base;

public readonly struct CompletionResult<TResult>
{
    public CompletionStatus CompletionStatus { get; }
    public TResult? Result { get; }

    public CompletionResult(CompletionStatus completionStatus, TResult? result)
    {
        CompletionStatus = completionStatus;
        Result = result;
    }
}