using Estudos.IdempotentConsumer.Base.Keys;
using Estudos.IdempotentConsumer.Enums;

namespace Estudos.IdempotentConsumer.Base;

public abstract class IdempotentConsumer<TMessage> : IIdempotentConsumer<TMessage>
    where TMessage : IIdempotentConsumerKey
{
    protected IMembershipStrategy<TMessage> MembershipStrategy { get; }


    public IdempotentConsumer(IMembershipStrategy<TMessage> membershipStrategy)
    {
        MembershipStrategy = membershipStrategy;
    }


    public virtual async Task<CompletionResult<TResult?>> ProcessAsync<TResult>(string instanceId, TMessage message, Func<Task<TResult?>> funcProcess)
    {
        if (await ExistsAsync(instanceId, message))
            return new CompletionResult<TResult?>(CompletionStatus.Ignored, default);

        try
        {
            await PreProcessAsync(instanceId, message);
            var result = await funcProcess.Invoke();
            await CommitAsync(instanceId, message);
            return new CompletionResult<TResult?>(CompletionStatus.Consumed, result);
        }
        catch (Exception)
        {
            await RollbackAsync(instanceId, message);
            throw;
        }
    }

    public virtual async Task<CompletionStatus> ProcessAsync(string instanceId, TMessage message, Func<Task> funcProcess)
    {
        if (await ExistsAsync(instanceId, message))
            return CompletionStatus.Ignored;

        try
        {
            await PreProcessAsync(instanceId, message);
            await funcProcess.Invoke();
            await CommitAsync(instanceId, message);
            return CompletionStatus.Consumed;
        }
        catch (Exception)
        {
            await RollbackAsync(instanceId, message);
            throw;
        }
    }

    protected virtual Task<bool> ExistsAsync(string instanceId, TMessage message)
    {
        return MembershipStrategy.ExistsAsync(instanceId, message);
    }

    protected virtual Task PreProcessAsync(string instanceId, TMessage message)
    {
        if (MembershipStrategy is IMembershipStrategyExtended<TMessage> membershipTestingStrategyExtended)
            return membershipTestingStrategyExtended.SetStateAsync(instanceId, message, RepositoryEntryState.Processing);

        return Task.CompletedTask;
    }

    protected virtual Task CommitAsync(string instanceId, TMessage message)
    {
        return MembershipStrategy.AddAsync(instanceId, message);
    }

    protected virtual Task RollbackAsync(string instanceId, TMessage message)
    {
        if (MembershipStrategy is IMembershipStrategyExtended<TMessage> membershipTestingStrategyExtended)
            return membershipTestingStrategyExtended.RemoveAsync(instanceId, message);

        return Task.CompletedTask;
    }
}