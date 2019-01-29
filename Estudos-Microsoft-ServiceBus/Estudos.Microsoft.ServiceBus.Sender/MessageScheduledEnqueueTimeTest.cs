using System;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;

namespace Estudos.Microsoft.ServiceBus.Sender
{
    public class MessageScheduledEnqueueTimeTest
    {
        static string connectionString = "Endpoint=sb://xpi-sdk-dsv-std.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=ZI9kfkj6SUDjdIEDRYlJa2tXLlRclujEo9U/QidVInE=";
        static string queueName = "corporate-security-fingerprint";

        public static async Task SendMessageAsync()
        {
            await using var client = new ServiceBusClient(connectionString);
            var sender = client.CreateSender(queueName);
            while (true)
            {
                Console.Write("Informe a descrição: ");
                var description = Console.ReadLine();
                var classTest = new MyClass(Guid.NewGuid(), description);
                var messageTest = new MessageTest<MyClass>(classTest);
                var serviceMessage = new ServiceBusMessage
                {
                    ContentType = MediaTypeNames.Application.Json,
                    MessageId = Guid.NewGuid().ToString(),
                    Body = new BinaryData(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(messageTest))),
                    ScheduledEnqueueTime = DateTimeOffset.Now.AddSeconds(-10)
                };
                
                await sender.SendMessageAsync(serviceMessage);
                Console.WriteLine($"Sent a single message to the queue: {queueName}");
            }
        }

        private class MessageTest<T> where T : class
        {
            private static int _id;

            public int Id => _id;

            public T Result { get; }
            
            public MessageTest(T result)
            {
                IncrementId();
                Result = result;
            }

            private static void IncrementId() => _id++;
        }
        
        private class MyClass
        {
            public Guid Id { get; }
            public string Description { get; }

            public MyClass(Guid id, string description)
            {
                Id = id;
                Description = description;
            }
        }
    }
}