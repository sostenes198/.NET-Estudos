namespace Estudos.IdempotentConsumer.Repositories.Slq.Base;

public interface ISqlService
{
    Task<IEnumerable<T>?> QueryListAsync<T>(string sql, object? param = default);

    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = default);

    Task<bool> ExistAsync(string sql, object? param = default);

    Task<int> InsertAsync(string sql, object? param = default);

    Task<int> UpdateAsync(string sql, object? param = default);

    Task<int> DeleteAsync(string sql, object? param = default);

}