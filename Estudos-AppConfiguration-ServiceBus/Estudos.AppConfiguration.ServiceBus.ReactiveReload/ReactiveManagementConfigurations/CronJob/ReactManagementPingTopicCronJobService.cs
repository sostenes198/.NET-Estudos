using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.CronJob;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.CronJob.Configs;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Configurations.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.CronJob
{
    [ExcludeFromCodeCoverage]
    internal sealed class ReactManagementPingTopicCronJobService : CronJobService
    {
        private readonly ILogger<ReactManagementPingTopicCronJobService> _logger;
        private readonly ReactManagementServiceBusConfiguration _reactManagementServiceBusConfiguration;

        public ReactManagementPingTopicCronJobService(ILogger<ReactManagementPingTopicCronJobService> logger, IScheduleConfig<ReactManagementPingTopicCronJobService> scheduleConfig,
            IConfiguration configuration)
            : base(scheduleConfig.CronExpression, scheduleConfig.TimeZoneInfo)
        {
            _logger = logger;

            _reactManagementServiceBusConfiguration = new ReactManagementServiceBusConfiguration();
            configuration.GetSection(ReactManagementServiceBusConfiguration.SectionName).Bind(_reactManagementServiceBusConfiguration);
        }

        protected override async Task DoWorkAsync(CancellationToken cancellationToken)
        {
            ServiceBusClient client = default;
            ServiceBusSender sender = default;
            try
            {
                client = new ServiceBusClient(_reactManagementServiceBusConfiguration.ConnectionString);
                sender = client.CreateSender(_reactManagementServiceBusConfiguration.Topic);

                await sender.SendMessageAsync(new ServiceBusMessage(), cancellationToken).ConfigureAwait(false);
                _logger.LogDebug("Mensagem de ping para consumidores enviada com sucesso.");
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Falha ao enviar mensagem de ping para consumidores: {message}", exception.Message);
            }
            finally
            {
                if (sender is not null)
                    await sender.DisposeAsync().ConfigureAwait(false);

                if (client is not null)
                    await client.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}