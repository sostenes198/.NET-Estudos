using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqConsumer.Capitulo_3_Publish_Subscribe
{
    public static class ReceiveLogs
    {
        public static void RecieveMessage()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channle = connection.CreateModel();
            channle.ExchangeDeclare("logs", ExchangeType.Fanout);

            var queueName = channle.QueueDeclare().QueueName;
            channle.QueueBind(queueName,
                "logs",
                "");
            
            Console.WriteLine(" [*] Waiting for logs.");


            var consumer = new EventingBasicConsumer(channle);
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] {0}", message);
            };
            channle.BasicConsume(
                queueName,
                true,
                consumer
            );

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}