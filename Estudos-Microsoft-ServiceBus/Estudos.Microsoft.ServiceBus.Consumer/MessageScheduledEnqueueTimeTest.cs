using System;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Estudos.Microsoft.ServiceBus.Consumer
{
    public class MessageScheduledEnqueueTimeTest
    {
        static string connectionString = "Endpoint=sb://xpi-sdk-dsv-std.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZI9kfkj6SUDjdIEDRYlJa2tXLlRclujEo9U/QidVInE=";
        static string topicName = "corporate-security-fingerprint";
        static string subscriptionName = "corporate-security-fingerprint-send-post-event";
        
        private static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            var result = args.Message.Body.ToObjectFromJson<MessageTest<MyClass>>();
            Console.WriteLine($"Received: {result}");

            // complete the message. messages is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        private static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        public static async Task ReceiveMessagesAsync()
        {
            await using (ServiceBusClient client = new ServiceBusClient(connectionString))
            {
                // create a processor that we can use to process the messages
                ServiceBusProcessor processor = client.CreateProcessor(topicName, subscriptionName, new ServiceBusProcessorOptions());

                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
                await processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
                await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
        }
        
        private class MessageTest<T> where T : class
        {

            public int Id { get; set; }

            public T Result { get; set; }

            public override string ToString()
            {
                return $"Id MessageClass: {Id} Result: {Result}";
            }
        }
        
        private class MyClass
        {
            public Guid Id { get; set; }
            public string Description { get; set; }

            public override string ToString()
            {
                return $"Id: {Id}, Description: {Description}";
            }
        }
    }
}