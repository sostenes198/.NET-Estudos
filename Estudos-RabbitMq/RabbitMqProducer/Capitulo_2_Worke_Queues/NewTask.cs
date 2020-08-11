using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMqProducer.Capitulo_2_Worke_Queues
{
    public class NewTask
    {
        public static void SendMessage()
        {
            var factory = new ConnectionFactory
            {
                HostName = "54.237.160.146",
                Password = "guest",
                UserName = "guest",
                Port = 5672
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.QueueDeclare(
                "task_queue",
                true,
                false,
                false,
                null
            );
            channel.BasicQos(0, 1, false);

            while (true)
            {
                Console.WriteLine("Write message: ");
                var args = Console.ReadLine();
                var message = GetMessage(args);
                var body = Encoding.UTF8.GetBytes(message);

                var properties = channel.CreateBasicProperties();
                properties.Persistent = true;

                channel.BasicPublish(
                    "",
                    "task_queue",
                    properties,
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