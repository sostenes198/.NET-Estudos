using System.Diagnostics.CodeAnalysis;
using Dapper;

namespace Estudos.IdempotentConsumer.Repositories.Slq.Base;

[ExcludeFromCodeCoverage]
internal sealed class SqlService : ISqlService
{
    private readonly IDataContext _dataContext;

    public SqlService(IDataContext dataContext)
    {
        _dataContext = dataContext;
    }

    public async Task<IEnumerable<T>?> QueryListAsync<T>(string sql, object? param = default)
    {
        using var connection = _dataContext.GetConnection();
        return await connection.QueryAsync<T>(sql, param);
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = default)
    {
        using var connection = _dataContext.GetConnection();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
    }

    public async Task<bool> ExistAsync(string sql, object? param = default)
    {
        using var connection = _dataContext.GetConnection();
        return await connection.ExecuteScalarAsync<bool>(sql, param);
    }

    public async Task<int> InsertAsync(string sql, object? param = default)
    {
        using var connection = _dataContext.GetConnection();
        return await connection.ExecuteAsync(sql, param);
    }

    public async Task<int> UpdateAsync(string sql, object? param = default)
    {
        using var connection = _dataContext.GetConnection();
        return await connection.ExecuteAsync(sql, param);
    }

    public async Task<int> DeleteAsync(string sql, object? param = default)
    {
        using var connection = _dataContext.GetConnection();
        return await connection.ExecuteAsync(sql, param);
    }
}