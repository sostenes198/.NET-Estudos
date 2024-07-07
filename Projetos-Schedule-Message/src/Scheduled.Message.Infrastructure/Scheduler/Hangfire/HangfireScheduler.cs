using Hangfire;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using Scheduled.Message.Application.Boundaries.Scheduler;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Configurations;
using Scheduled.Message.Infrastructure.Scheduler.Hangfire.Models;

namespace Scheduled.Message.Infrastructure.Scheduler.Hangfire;

public sealed class HangfireScheduler(
    IServiceProvider serviceProvider,
    IOptions<HangfireConfigurations> options,
    IBackgroundJobClient jobClient,
    ILogger<HangfireScheduler> logger)
    : IScheduler
{
    private readonly HangfireConfigurations _hangfireConfigurations = options.Value;

    public async Task ScheduleAsync<TInput, THandler>(string jobName, TInput input, TimeSpan delayToWait,
        CancellationToken token)
        where TInput : class
        where THandler : ISchedulerHandler<TInput>
    {
        var idResult = await SaveParamsAsync(input, token);

        jobClient.Schedule<IHangifireCallbackHandler<THandler, TInput>>(lnq =>
            lnq.ExecuteAsync(idResult.ToString(), jobName), delayToWait);

        logger.LogInformation("Request scheduled successfully {id}", jobName);
    }

    private async Task<ObjectId> SaveParamsAsync<TInput>(TInput input, CancellationToken token)
        where TInput : class
    {
        var repository = serviceProvider.GetRequiredService<IHangfireSchedulerRepository<TInput>>();

        var hangfireParams = new HangfireParams<TInput>
        {
            Id = ObjectId.GenerateNewId(),
            Input = input,
            ExpireAt = DateTime.UtcNow.AddDays(_hangfireConfigurations.TtlHangfireDocumentInDays)
        };

        await repository.SaveParamsAsync(hangfireParams, token);

        logger.LogInformation("Parameters successfully saved");

        return hangfireParams.Id;
    }
}