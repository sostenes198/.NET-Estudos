using Estudos.IdempotentConsumer.Options;
using Estudos.IdempotentConsumer.Repositories.Slq.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Estudos.IdempotentConsumer.Tests.Integration.Repositories.Slq.Base;

public class BaseSqlRepositoryTest : BaseTest
{
    public BaseSqlRepositoryTest()
    {
        ConfigureServices += (context, collection) =>
        {
            collection.Configure<SqlRepositoryOptions>(opt =>
            {
                opt.ConnectionString = context.Configuration[AppSettingsConstants.SqlServerConnectionString];
            });
            collection.TryAddSingleton<IDataContext, DataContext>();
            collection.TryAddScoped<ISqlService, SqlService>();
        };
    }
}