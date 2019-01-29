using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.CodeAnalysis;
using Estudos.IdempotentConsumer.Options;
using Microsoft.Extensions.Options;

namespace Estudos.IdempotentConsumer.Repositories.Slq.Base;

[ExcludeFromCodeCoverage]
internal sealed class DataContext : IDataContext
{
    private readonly string _connectionString; 

    public IDbConnection GetConnection() => new SqlConnection(_connectionString);

    public DataContext(IOptions<SqlRepositoryOptions> options)
    {
        _connectionString = options.Value.ConnectionString;
    }
}