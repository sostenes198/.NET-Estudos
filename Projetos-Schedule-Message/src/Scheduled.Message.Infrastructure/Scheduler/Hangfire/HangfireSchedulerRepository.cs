using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Scheduled.Message.Infrastructure.Databases.Mongo.Clients;
using Scheduled.Message.Infrastructure.Databases.Mongo.Configurations;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases.Hangfire;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Models;

namespace Scheduled.Message.Infrastructure.Scheduler.Hangfire;

public class HangfireSchedulerRepository<TInput>(
    IMongoDatabaseClient<AppMongoDatabaseHangfire> mongoDatabaseHangfireClient,
    IOptions<MongoConfigurations> mongoOptions)
    : IHangfireSchedulerRepository<TInput>
    where TInput : class
{
    private string Collection => mongoOptions.Value.Hangfire.HangfireParametersCollection;

    public Task SaveParamsAsync(HangfireParams<TInput> hangfireParams, CancellationToken token)
    {
        return mongoDatabaseHangfireClient.CreateAsync(Collection,
            hangfireParams, default, token);
    }

    public Task<HangfireParams<TInput>> RecoveryParamsAsync(string id, CancellationToken token)
    {
        return mongoDatabaseHangfireClient.GetAsync(Collection,
            new ExpressionFilterDefinition<HangfireParams<TInput>>(lnq => lnq.Id == new ObjectId(id)),
            default,
            token
        )!;
    }
}