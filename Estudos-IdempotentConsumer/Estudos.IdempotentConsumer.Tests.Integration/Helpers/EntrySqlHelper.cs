using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Estudos.IdempotentConsumer.Repositories.Base;
using Estudos.IdempotentConsumer.Repositories.Slq.Base;

namespace Estudos.IdempotentConsumer.Tests.Integration.Helpers;

public static class EntrySqlHelper
{
    private const string SqlItemPropertiesQuery = @"
        [INSTANCE_ID] as InstanceId,
        [IDEMPOTENCY_KEY] as IdempotencyKey,
        [STATE] as State,
        [TIMESTAMP] as Timestamp
    ";

    private const string ListAllItemsQuery = @$"            
        SELECT  
            {SqlItemPropertiesQuery}
        FROM [dbo].[IDEMPOTENCY_CONSUMER] WITH (NOLOCK)
    ";

    private const string GetItemsQuery = @$"            
        SELECT  
            {SqlItemPropertiesQuery}
        FROM [dbo].[IDEMPOTENCY_CONSUMER] WITH (NOLOCK)
        WHERE [INSTANCE_ID] = @instanceId AND [IDEMPOTENCY_KEY] = @idempotencyKey
    ";

    private const string DeleteItemQuery = @"
        DELETE FROM [dbo].[IDEMPOTENCY_CONSUMER] WHERE [INSTANCE_ID] = @instanceId AND [IDEMPOTENCY_KEY] = @idempotencyKey
    ";

    public static async Task<IEnumerable<Entry>> ListAsync(IDataContext dataContext)
    {
        using var connection = dataContext.GetConnection();
        return await connection.QueryAsync<Entry>(ListAllItemsQuery);
    }

    public static async Task<Entry> GetAsync(IDataContext dataContext, string instanceId, string idempotencyKey)
    {
        using var connection = dataContext.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<Entry>(GetItemsQuery, new {instanceId = instanceId, idempotencyKey = idempotencyKey});
    }

    public static async Task DeleteAsync(IDataContext dataContext, string instanceId, string idempotencyKey)
    {
        using var connection = dataContext.GetConnection();
        await connection.ExecuteAsync(DeleteItemQuery, new {instanceId = instanceId, idempotencyKey = idempotencyKey});
    }
}