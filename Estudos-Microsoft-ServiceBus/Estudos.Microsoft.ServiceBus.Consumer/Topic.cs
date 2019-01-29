using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Estudos.Microsoft.ServiceBus.Consumer
{
    public static class Topic
    {
        static string connectionString = "Endpoint=sb://estudos-soso.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=HDFQfZPpUAelamHitQeZ0c+e2FG2O7arW49QfLwxHtM=";
        static string topicName = "teste-topic";
        static string subscriptionName = "corporate-security-fingerprint-send-post-event";
        
        public static async Task ReceiveMessagesFromSubscriptionAsync(string subscriptionName)
        {
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                // create a processor that we can use to process the messages
                ServiceBusProcessor processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

                // add handler to process messages
                processor.ProcessMessageAsync += args => MessageHandler(args, subscriptionName);

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await processor.StartProcessingAsync();

                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
        }
        
        static async Task MessageHandler(ProcessMessageEventArgs args, string subscriptionName)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body} from subscription: {subscriptionName}");

            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }
    }
}