using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqConsumer.Capitulo_1_Introducao
{
    public static class Recieve
    {
        public static void RecieveMessage()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channle = connection.CreateModel();
            channle.QueueDeclare(
                "hello",
                false,
                false,
                false,
                null
            );

            var consumer = new EventingBasicConsumer(channle);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);
            };
            channle.BasicConsume(
                "hello",
                true,
                consumer
            );

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}