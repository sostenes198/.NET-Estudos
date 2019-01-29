using System.Data;

namespace Estudos.IdempotentConsumer.Repositories.Slq.Base;

public interface IDataContext
{
    IDbConnection GetConnection();
}