using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMqProducer.Capitulo_3_Publish_Subscribe
{
    public static class EmitLog
    {
        public static void SendMessage()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare("logs", ExchangeType.Fanout);

            while (true)
            {
                Console.WriteLine("Write message: ");
                var args = Console.ReadLine();
                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    "logs",
                    "",
                    null,
                    body
                );

                Console.WriteLine(" [x] Sent {0}", message);
            }
        }

        private static string GetMessage(string args)
        {
            return (args.Length > 0) ? string.Join(" ", args) : "Hello World!";
        }
    }
}