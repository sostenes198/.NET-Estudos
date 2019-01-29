using System;
using System.Threading.Tasks;
using Azure.Messaging.EventGrid;
using Microsoft.Azure.ServiceBus;
using Microsoft.Azure.ServiceBus.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration;
using Microsoft.Extensions.Configuration.AzureAppConfiguration.Extensions;

namespace Estudos.AppConfiguration.ServiceBus
{
    class Program
    {
        private const string AppConfigurationConnectionString = ".";
        private const string ServiceBusConnectionString = ".";
        private const string ServiceBusTopic = ".";
        private const string ServiceBusSubscription = ".";

        private static IConfigurationRefresher _refresher;

        static async Task Main()
        {
            IConfiguration configuration = new ConfigurationBuilder()
                .AddAzureAppConfiguration(options =>
                {
                    options.Connect(AppConfigurationConnectionString);
                    options.ConfigureRefresh(refresh =>
                    {
                        refresh.Register("TestApp:Settings:Message", true);
                        refresh.SetCacheExpiration(TimeSpan.FromDays(30));
                    });
                    options.UseFeatureFlags(flagOptions => { flagOptions.CacheExpirationInterval = TimeSpan.FromDays(30); });
                    _refresher = options.GetRefresher();
                }).Build();

            await RegisterRefreshEventHandler();
            var message = configuration["TestApp:Settings:Message"];
            Console.WriteLine($"Message Initial value: {configuration["TestApp:Settings:Message"]}");

            var featureFlagValue = configuration["FeatureManagement:Teste"];
            Console.WriteLine($"FeatureFlag Initial value: {configuration["FeatureManagement:Teste"]}");

            while (true)
            {
                if (configuration["TestApp:Settings:Message"] != message)
                {
                    Console.WriteLine($"Message New value: {configuration["TestApp:Settings:Message"]}");
                    message = configuration["TestApp:Settings:Message"];
                }
                
                if (configuration["FeatureManagement:Teste"] != featureFlagValue)
                {
                    Console.WriteLine($"FeatureFlag New value: {configuration["FeatureManagement:Teste"]}");
                    featureFlagValue = configuration["FeatureManagement:Teste"];
                }

                await Task.Delay(TimeSpan.FromSeconds(1));
            }
        }

        private static async Task RegisterRefreshEventHandler()
        {
            string serviceBusConnectionString = ServiceBusConnectionString;
            string serviceBusTopic = ServiceBusTopic;
            string serviceBusSubscription = ServiceBusSubscription;
            try
            {

                var managementClient = new ManagementClient(ServiceBusConnectionString);
                if (!await managementClient.SubscriptionExistsAsync(serviceBusTopic, serviceBusSubscription))
                {
                    throw new Exception("FAIO");
                }

                var subscriptionClient = new SubscriptionClient(
                    serviceBusConnectionString,
                    serviceBusTopic,
                    serviceBusSubscription);

                subscriptionClient.RegisterMessageHandler(
                    async (message, cancellationToken) =>
                    {
                        // Build EventGridEvent from notification message
                        EventGridEvent eventGridEvent = EventGridEvent.Parse(BinaryData.FromBytes(message.Body));

                        // Create PushNotification from eventGridEvent
                        eventGridEvent.TryCreatePushNotification(out PushNotification pushNotification);

                        // Prompt Configuration Refresh based on the PushNotification
                        _refresher.ProcessPushNotification(pushNotification, TimeSpan.Zero);
                        
                        await _refresher.TryRefreshAsync(cancellationToken);
                    },
                    exceptionArgs =>
                    {
                        Console.WriteLine($"{exceptionArgs.Exception}");
                        return Task.CompletedTask;
                    });
            }
            catch (Exception exception)
            {
            }
        }
    }
}