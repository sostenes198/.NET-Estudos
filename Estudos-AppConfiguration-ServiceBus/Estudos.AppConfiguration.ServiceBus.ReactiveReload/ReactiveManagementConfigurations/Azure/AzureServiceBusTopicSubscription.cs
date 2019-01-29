using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Azure.Messaging.ServiceBus;
using Azure.Messaging.ServiceBus.Administration;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Configurations.ServiceBus;
using Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.Extensions;
using Microsoft.Extensions.Logging;

namespace Estudos.AppConfiguration.ServiceBus.ReactiveReload.ReactiveManagementConfigurations.Azure
{
    [ExcludeFromCodeCoverage]
    internal sealed class AzureServiceBusTopicSubscription : IAzureServiceBusTopicSubscription
    {
        private readonly ILogger<AzureServiceBusTopicSubscription> _logger;
        private readonly ServiceBusClient _serviceBusClient;
        private readonly ServiceBusAdministrationClient _serviceBusAdministrationClient;

        private ServiceBusProcessor _processor;

        private readonly string _topic;
        private readonly string _subscriptionName;

        public AzureServiceBusTopicSubscription(IConfiguration configuration, ILogger<AzureServiceBusTopicSubscription> logger)
        {
            _logger = logger;
            var reactManagementServiceBusConfiguration = new ReactManagementServiceBusConfiguration();
            configuration.GetSection(ReactManagementServiceBusConfiguration.SectionName).Bind(reactManagementServiceBusConfiguration);

            _topic = reactManagementServiceBusConfiguration.Topic;
            _subscriptionName = GetSubscriptionName(reactManagementServiceBusConfiguration.ApplicationName);
            _serviceBusClient = new ServiceBusClient(reactManagementServiceBusConfiguration.ConnectionString);
            _serviceBusAdministrationClient = new ServiceBusAdministrationClient(reactManagementServiceBusConfiguration.ConnectionString);
        }

        public async Task CreateProcessorAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                await ValidateIfTopicExistAsync(cancellationToken).ConfigureAwait(false);
                await CreateSubscriptionIfNotExistAsync(cancellationToken).ConfigureAwait(false);


                _processor = _serviceBusClient.CreateProcessor(_topic, _subscriptionName, new ServiceBusProcessorOptions
                {
                    MaxConcurrentCalls = 1,
                    AutoCompleteMessages = false
                });

                _processor.ProcessMessageAsync += ProcessorOnProcessMessageAsync;
                _processor.ProcessErrorAsync += ProcessorOnProcessErrorAsync;

                await _processor.StartProcessingAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Error ao configurar o servicebus: {message}", exception.Message);
            }
        }

        private string GetSubscriptionName(string applicationName)
        {
            var subscriptionName = $"{applicationName}-{ShortGuid.NewGuid()}";
            _logger.LogInformation("Subscription {subscriptionName} criado para a máquina {machineName}", subscriptionName, Environment.MachineName);
            return subscriptionName;
        }

        private async Task ValidateIfTopicExistAsync(CancellationToken cancellationToken)
        {
            if (!await _serviceBusAdministrationClient.TopicExistsAsync(_topic, cancellationToken).ConfigureAwait(false))
                throw new Exception($"Topico {_topic} não encontrado");
        }

        private async Task CreateSubscriptionIfNotExistAsync(CancellationToken cancellationToken)
        {
            if (!await _serviceBusAdministrationClient.SubscriptionExistsAsync(_topic, _subscriptionName, cancellationToken).ConfigureAwait(false))
            {
                _logger.LogDebug("Subscription {subscription} não encontrado no topico {topic}, criando subscription", _subscriptionName, _topic);
                await _serviceBusAdministrationClient.CreateSubscriptionAsync(new CreateSubscriptionOptions(_topic, _subscriptionName)
                {
                    MaxDeliveryCount = 3,
                    LockDuration = TimeSpan.FromMinutes(1),
                    AutoDeleteOnIdle = TimeSpan.FromDays(5),
                    DefaultMessageTimeToLive = TimeSpan.FromDays(1)
                }, cancellationToken).ConfigureAwait(false);
            }
        }

        private async Task ProcessorOnProcessMessageAsync(ProcessMessageEventArgs arg)
        {
            if (arg.Message.Body.ToArray().Length == 0)
            {
                await arg.CompleteMessageAsync(arg.Message).ConfigureAwait(false);
                return;
            }
            
            _logger.LogDebug("Mensagem recebida {bodyMessage}", arg.Message.Body.ToString());

            EventGridEvent eventGridEvent = EventGridEvent.Parse(BinaryData.FromBytes(arg.Message.Body));
            eventGridEvent.TryCreatePushNotification(out PushNotification pushNotification);

            AzureSettings.AzureConfigurationRefresher.ProcessPushNotification(pushNotification, TimeSpan.Zero);
            await AzureSettings.AzureConfigurationRefresher.RefreshAsync().ConfigureAwait(false);

            await arg.CompleteMessageAsync(arg.Message).ConfigureAwait(false);
        }

        private Task ProcessorOnProcessErrorAsync(ProcessErrorEventArgs arg)
        {
            _logger.LogError(arg.Exception, "Falha ao processar mensagem {message}", arg.Exception.Message);
            return Task.CompletedTask;
        }

        public async Task TryReconnectAsync()
        {
            if (_processor is not null && _processor.IsClosed)
                await CreateProcessorAsync(CancellationToken.None).ConfigureAwait(false);
        }

        public async Task CloseProcessorAsync(CancellationToken cancellationToken = default)
        {
            await TryRemoveSubscriptionAsync(cancellationToken).ConfigureAwait(false);
            await TryCloseProcessor(cancellationToken).ConfigureAwait(false);
        }

        private async Task TryRemoveSubscriptionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (await _serviceBusAdministrationClient.SubscriptionExistsAsync(_topic, _subscriptionName, cancellationToken).ConfigureAwait(false))
                    await _serviceBusAdministrationClient.DeleteSubscriptionAsync(_topic, _subscriptionName, cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Falha ao deletar subscription {subscription}. {message}", _subscriptionName, exception.Message);
            }
        }

        private async Task TryCloseProcessor(CancellationToken cancellationToken = default)
        {
            try
            {
                if (_processor is not null)
                    await _processor.CloseAsync(cancellationToken).ConfigureAwait(false);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "Falha ao tentar pausar processor: {message}", exception.Message);
            }
        }

        public async ValueTask DisposeAsync()
        {
            if (_processor is not null)
                await _processor.DisposeAsync().ConfigureAwait(false);

            if (_serviceBusClient is not null)
                await _serviceBusClient.DisposeAsync().ConfigureAwait(false);
        }
    }
}