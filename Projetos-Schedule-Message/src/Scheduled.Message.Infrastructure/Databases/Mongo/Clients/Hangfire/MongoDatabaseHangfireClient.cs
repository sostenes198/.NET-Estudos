using Scheduled.Message.Infrastructure.Databases.Mongo.Databases.Hangfire;

namespace Scheduled.Message.Infrastructure.Databases.Mongo.Clients.Hangfire;

public class MongoDatabaseHangfireClient(AppMongoDatabaseHangfire appMongoDatabase)
    : MongoDatabaseClient<AppMongoDatabaseHangfire>(appMongoDatabase)
{
}