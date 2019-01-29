using Estudos.IdempotentConsumer.Repositories.Base;
using Estudos.IdempotentConsumer.Repositories.CircularBuffer;
using Estudos.IdempotentConsumer.Repositories.Slq.Base;

namespace Estudos.IdempotentConsumer.Repositories.Slq;

internal sealed class SqlServerRepository : IRepository
{
    private const string QuerySqlColumnsToSelect = @"
        [INSTANCE_ID] as InstanceId,
        [IDEMPOTENCY_KEY] as IdempotencyKey,
        [STATE] as State,
        [TIMESTAMP] as Timestamp
    ";

    public const string GetElement = @$"
        SELECT TOP(1) 
                {QuerySqlColumnsToSelect}
        FROM [IDEMPOTENCY_CONSUMER]
        WHERE [INSTANCE_ID] = @instanceId AND [IDEMPOTENCY_KEY] = @idempotencyKey
    ";

    public const string ListElements = @$"
        SELECT TOP(@dataFetchThreshold)
                {QuerySqlColumnsToSelect}
        FROM [IDEMPOTENCY_CONSUMER]
        WHERE [INSTANCE_ID] = @instanceId
        ORDER BY [TIMESTAMP]
    ";

    public const string ContainsElement = @"SELECT COUNT(1) FROM [IDEMPOTENCY_CONSUMER] WHERE [INSTANCE_ID] = @instanceId AND [IDEMPOTENCY_KEY] = @idempotencyKey";

    public const string InsertElement = @"
        INSERT INTO [IDEMPOTENCY_CONSUMER] (INSTANCE_ID, IDEMPOTENCY_KEY, STATE, TIMESTAMP)
            VALUES(@instanceId, @idempotencyKey, @state, @timestamp)
    ";

    public const string UpdateElement = @"
        UPDATE [IDEMPOTENCY_CONSUMER] SET            
            [STATE] = @state,
            [TIMESTAMP] = @timestamp
        WHERE [INSTANCE_ID] = @instanceId AND [IDEMPOTENCY_KEY] = @idempotencyKey
    ";

    public const string DeleteElement = @"
        DELETE FROM [IDEMPOTENCY_CONSUMER] WHERE [INSTANCE_ID] = @instanceId AND [IDEMPOTENCY_KEY] = @idempotencyKey
    ";

    private readonly ISqlService _sqlService;
    private readonly IInMemoryCircularBufferRepository _inMemoryCircularBufferRepository;

    public SqlServerRepository(ISqlService sqlService, IInMemoryCircularBufferRepository inMemoryCircularBufferRepository)
    {
        _sqlService = sqlService;
        _inMemoryCircularBufferRepository = inMemoryCircularBufferRepository;
    }
    
    public async Task<bool> ContainsAsync(string instanceId, string idempotencyKey)
    {
        return await _inMemoryCircularBufferRepository.ContainsAsync(instanceId, idempotencyKey) || await InternalExistAsync(instanceId, idempotencyKey);
    }

    public async Task<Entry> GetEntryAsync(string instanceId, string idempotencyKey)
    {
        var result = await _sqlService.QueryFirstOrDefaultAsync<Entry>(GetElement, new
        {
            instanceId = instanceId,
            idempotencyKey = idempotencyKey
        });

        return result ?? Entry.Empty;
    }

    public async Task<IEnumerable<Entry>> GetEntriesAsync(string instanceId, int dataFetchThreshold)
    {
        var result = await _sqlService.QueryListAsync<Entry>(ListElements, new
        {
            instanceId = instanceId,
            dataFetchThreshold = dataFetchThreshold
        });

        return result ?? Array.Empty<Entry>();
    }

    public async Task AddOrUpdateAsync(Entry data)
    {
        var paramData = new
        {
            instanceId = data.InstanceId,
            idempotencyKey = data.IdempotencyKey,
            state = data.State.ToString(),
            timestamp = data.Timestamp
        };

        await _inMemoryCircularBufferRepository.AddOrUpdateAsync(data);

        if (await InternalExistAsync(data.InstanceId, data.IdempotencyKey))
        {
            await _sqlService.UpdateAsync(UpdateElement, paramData);
            return;
        }

        await _sqlService.InsertAsync(InsertElement, paramData);
    }

    public Task RemoveAsync(string instanceId, string idempotencyKey)
    {
        var taskInMemoryDelete = _inMemoryCircularBufferRepository.RemoveAsync(instanceId, idempotencyKey);
        var taskSqlServiceDelete = _sqlService.DeleteAsync(DeleteElement, new
        {
            InstanceId = instanceId,
            IdempotencyKey = idempotencyKey
        });
        return Task.WhenAll(taskInMemoryDelete, taskSqlServiceDelete);
    }

    private Task<bool> InternalExistAsync(string instanceId, string idempotencyKey) => _sqlService.ExistAsync(ContainsElement, new {instanceId = instanceId, idempotencyKey = idempotencyKey});
}