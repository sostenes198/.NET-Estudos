using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

namespace Estudos.Microsoft.ServiceBus.Sender
{
    public static class Message
    {
        static string connectionString = "Endpoint=sb://estudos-soso.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=HDFQfZPpUAelamHitQeZ0c+e2FG2O7arW49QfLwxHtM=";
        static string queueName = "teste-queue";
        
        public static async Task SendMessageAsync()
        {
            // create a Service Bus client 
            await using ServiceBusClient client = new ServiceBusClient(connectionString);
            // create a sender for the queue 
            ServiceBusSender sender = client.CreateSender(queueName);

            // create a message that we can send
            ServiceBusMessage message = new ServiceBusMessage("Hello world!");

            // send the message
            await sender.SendMessageAsync(message);
            Console.WriteLine($"Sent a single message to the queue: {queueName}");
        }

        public static async Task SendMessageBatchAsync()
        {
            // create a Service Bus client 
            await using ServiceBusClient client = new ServiceBusClient(connectionString);
            // create a sender for the queue 
            ServiceBusSender sender = client.CreateSender(queueName);

            // get the messages to be sent to the Service Bus queue
            Queue<ServiceBusMessage> messages = CreateMessages();

            // total number of messages to be sent to the Service Bus queue
            int messageCount = messages.Count;

            // while all messages are not sent to the Service Bus queue
            while (messages.Count > 0)
            {
                // start a new batch 
                using ServiceBusMessageBatch messageBatch = await sender.CreateMessageBatchAsync();

                // add the first message to the batch
                if (messageBatch.TryAddMessage(messages.Peek()))
                {
                    // dequeue the message from the .NET queue once the message is added to the batch
                    messages.Dequeue();
                }
                else
                {
                    // if the first message can't fit, then it is too large for the batch
                    throw new Exception($"Message {messageCount - messages.Count} is too large and cannot be sent.");
                }

                // add as many messages as possible to the current batch
                while (messages.Count > 0 && messageBatch.TryAddMessage(messages.Peek()))
                {
                    // dequeue the message from the .NET queue as it has been added to the batch
                    messages.Dequeue();
                }

                // now, send the batch
                await sender.SendMessagesAsync(messageBatch);

                // if there are any remaining messages in the .NET queue, the while loop repeats 
            }

            Console.WriteLine($"Sent a batch of {messageCount} messages to the topic: {queueName}");
        }

        static Queue<ServiceBusMessage> CreateMessages()
        {
            // create a queue containing the messages and return it to the caller
            Queue<ServiceBusMessage> messages = new Queue<ServiceBusMessage>();
            messages.Enqueue(new ServiceBusMessage("First message in the batch"));
            messages.Enqueue(new ServiceBusMessage("Second message in the batch"));
            messages.Enqueue(new ServiceBusMessage("Third message in the batch"));
            return messages;
        }
    }
}