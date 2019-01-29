using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqConsumer.Capitulo_5_Topics
{
    public static class ReceiveLogsTopic
    {
        public static void RecieveMessage(string[] args)
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();
            channel.ExchangeDeclare("topic_logs", ExchangeType.Topic);

            var queueName = channel.QueueDeclare().QueueName;
            
            if(args.Length < 1)
            {
                Console.Error.WriteLine("Usage: {0} [binding_key...]",
                    Environment.GetCommandLineArgs()[0]);
                Console.WriteLine(" Press [enter] to exit.");
                Console.ReadLine();
                Environment.ExitCode = 1;
                return;
            }
            
            foreach(var bindingKey in args)
            {
                channel.QueueBind(queueName,
                    "topic_logs",
                    bindingKey);
            }
            
            Console.WriteLine(" [*] Waiting for messages. To exit press CTRL+C");
            
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine(" [x] Received '{0}':'{1}'",
                    routingKey,
                    message);
            };
            channel.BasicConsume(queueName,
                true,
                consumer);

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}