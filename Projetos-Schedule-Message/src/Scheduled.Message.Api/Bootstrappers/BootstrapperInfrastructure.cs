using System.Diagnostics.CodeAnalysis;
using Flurl.Http.Configuration;
using Hangfire;
using Hangfire.Mongo;
using Hangfire.Mongo.Migration.Strategies;
using Hangfire.Mongo.Migration.Strategies.Backup;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Scheduled.Message.Application.Boundaries.Gateways.VollScheduler;
using Scheduled.Message.Application.Boundaries.Scheduler;
using Scheduled.Message.Application.Boundaries.UseCases;
using Scheduled.Message.Application.Boundaries.UseCases.Validators;
using Scheduled.Message.Infrastructure.Databases.Mongo.Clients;
using Scheduled.Message.Infrastructure.Databases.Mongo.Clients.Hangfire;
using Scheduled.Message.Infrastructure.Databases.Mongo.Configurations;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases;
using Scheduled.Message.Infrastructure.Databases.Mongo.Databases.Hangfire;
using Scheduled.Message.Infrastructure.Gateways.Configurations;
using Scheduled.Message.Infrastructure.Gateways.VollScheduler;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Configurations;
using Scheduled.Message.Infrastructure.UseCases;
using Scheduled.Message.Infrastructure.UseCases.Validators;

namespace Scheduled.Message.Api.Bootstrappers;

[ExcludeFromCodeCoverage]
internal static class BootstrapperInfrastructure
{
    internal static IServiceCollection InitializeInfrastructure(this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        return services
            .InitializeHangfire(configuration)
            .InitializeDatabase(configuration)
            .InitializeUseCase()
            .InitializeScheduler()
            .InitializeGateways(configuration);
    }

    private static IServiceCollection InitializeHangfire(this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        services.AddOptions<HangfireConfigurations>()
            .Bind(configuration.GetSection(HangfireConfigurations.Section))
            .ValidateDataAnnotations()
            .ValidateOnStart();
        
        services.TryAddScoped(typeof(IHangifireCallbackHandler<,>), typeof(HangifireCallbackHandler<,>));

        services
            .AddHangfire((provider, configuration) =>
            {
                var mongoDatabaseHangfire = provider.GetRequiredService<AppMongoDatabaseHangfire>();
                var hangfireOptions = provider.GetRequiredService<IOptions<HangfireConfigurations>>().Value;

                configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseMongoStorage(mongoDatabaseHangfire.GetMongoClient(),
                        mongoDatabaseHangfire.GetMongoUrlBuilder().DatabaseName,
                        new MongoStorageOptions
                        {
                            MigrationOptions = new MongoMigrationOptions
                            {
                                MigrationStrategy = new MigrateMongoMigrationStrategy(),
                                BackupStrategy = new CollectionMongoBackupStrategy()
                            },
                            CheckQueuedJobsStrategy = CheckQueuedJobsStrategy.Poll,
                            SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
                            Prefix = "hangfire",
                            CheckConnection = true,
                            JobExpirationCheckInterval = TimeSpan.FromHours(6),
                        })
                    .WithJobExpirationTimeout(TimeSpan.FromDays(hangfireOptions.TtlHangfireDocumentInDays));
            });

        services.AddHangfireServer(opt =>
        {
            opt.ServerName = String.Format(
                "{0}.{1}",
                Environment.MachineName,
                Guid.NewGuid().ToString());
        });

        return services;
    }

    private static IServiceCollection InitializeDatabase(this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        services.AddOptions<MongoConfigurations>()
            .Bind(configuration.GetSection(MongoConfigurations.Section))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.InitializeMongoDbHangfire();
        services.TryAddSingleton<IMongoDatabaseClient<AppMongoDatabaseHangfire>, MongoDatabaseHangfireClient>();
        return services;
    }

    private static IServiceCollection InitializeMongoDbHangfire(this IServiceCollection services)
    {
        services.TryAddSingleton<AppMongoDatabaseHangfire>(provider =>
        {
            var mongoConfigurations = provider.GetRequiredService<IOptions<MongoConfigurations>>().Value;
            var mongoUrlBuilder = new MongoUrlBuilder($"{mongoConfigurations.Hangfire.ConnectionString}");
            var settings = MongoClientSettings.FromConnectionString(mongoUrlBuilder.ToString());
            var mongoClient = new MongoClient(settings);

            return new AppMongoDatabaseHangfire(mongoClient,
                mongoUrlBuilder,
                new List<string>
                {
                    mongoConfigurations.Hangfire.HangfireParametersCollection
                });
        });
        services.AddSingleton<IAppMongoDatabase>(provider => provider.GetRequiredService<AppMongoDatabaseHangfire>());

        services.TryAddScoped(typeof(IHangfireSchedulerRepository<>), typeof(HangfireSchedulerRepository<>));

        return services;
    }

    private static IServiceCollection InitializeUseCase(this IServiceCollection services)
    {
        services.TryAddScoped<IUseCaseManager, UseCaseManager>();
        services.TryAddSingleton(typeof(IUseCaseInputValidator<>), typeof(UseCaseInputValidator<>));
        return services;
    }

    private static IServiceCollection InitializeScheduler(this IServiceCollection services)
    {
        services.TryAddScoped<IScheduler, HangfireScheduler>();
        return services;
    }

    private static IServiceCollection InitializeGateways(this IServiceCollection services,
        IConfigurationRoot configuration)
    {
        services.AddOptions<GatewayConfigurations>()
            .Bind(configuration.GetSection(GatewayConfigurations.Section))
            .ValidateDataAnnotations()
            .ValidateOnStart();

        services.TryAddSingleton<IFlurlClientCache>(provider =>
        {
            var gatewayConfigurations = provider.GetRequiredService<IOptions<GatewayConfigurations>>().Value;

            return new FlurlClientCache()
                .Add(GatewayConfigurationsVollConfigurations.ClientName, gatewayConfigurations.VollScheduler.BaseUrl);
        });

        services.TryAddScoped<IVollSchedulerGateway, VollSchedulerGateway>();

        return services;
    }
}