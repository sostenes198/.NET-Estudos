using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMqProducer.Capitulo_4_Routing
{
    public static class EmitLogDirect
    {
        public static void SendMessage()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare("direct_logs", ExchangeType.Direct);

            while (true)
            {
                var severity = new Random().Next(1, 3).ToString();
                Console.WriteLine("Write message: ");
                var args = Console.ReadLine();
                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                channel.BasicPublish(
                    "direct_logs",
                    severity,
                    null,
                    body
                );

                Console.WriteLine(" [x] Sent {0} Seveirty {1}", message, severity);
            }
        }

        private static string GetMessage(string args)
        {
            return (args.Length > 0) ? string.Join(" ", args) : "Hello World!";
        }
    }
}