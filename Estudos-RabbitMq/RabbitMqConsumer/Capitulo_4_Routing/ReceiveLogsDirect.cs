using System;
using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqConsumer.Capitulo_4_Routing
{
    public static class ReceiveLogsDirect
    {
        public static void RecieveMessage()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channle = connection.CreateModel();
            channle.ExchangeDeclare("direct_logs", ExchangeType.Direct);

            var queueName = channle.QueueDeclare().QueueName;
            for (int i = 0; i < 3; i++)
            {
                channle.QueueBind(queueName,
                    "direct_logs",
                    i.ToString());
            }

            Console.WriteLine(" [*] Waiting for logs.");


            var consumer = new EventingBasicConsumer(channle);
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine(" [x] Received '{0}':'{1}'",
                    routingKey, message);
            };
            channle.BasicConsume(
                queueName,
                true,
                consumer
            );

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }

        public static void RecieveMessage1()
        {
            var factory1 = new ConnectionFactory {HostName = "localhost"};
            using var connection1 = factory1.CreateConnection();
            using var channle1 = connection1.CreateModel();
            channle1.ExchangeDeclare("direct_logs", ExchangeType.Direct);

            var queueName1 = channle1.QueueDeclare().QueueName;
            channle1.QueueBind(queueName1,
                "direct_logs",
                1.ToString());

            Console.WriteLine("Severity [1] [*] Waiting for logs [1]. \n");


            var consumer1 = new EventingBasicConsumer(channle1);
            consumer1.Received += (sender, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine("Severity [1] [x] Received '{0}':'{1}' \n",
                    routingKey, message);
            };
            channle1.BasicConsume(
                queueName1,
                true,
                consumer1
            );

            var factory2 = new ConnectionFactory {HostName = "localhost"};
            using var connection2 = factory2.CreateConnection();
            using var channle2 = connection2.CreateModel();
            channle2.ExchangeDeclare("direct_logs", ExchangeType.Direct);

            var queueName2 = channle2.QueueDeclare().QueueName;
            channle2.QueueBind(queueName2,
                "direct_logs",
                2.ToString());

            Console.WriteLine("Severity [2] [*] Waiting for logs [2]. \n");


            var consumer2 = new EventingBasicConsumer(channle2);
            consumer2.Received += (sender, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                var routingKey = ea.RoutingKey;
                Console.WriteLine("Severity [2] [x] Received '{0}':'{1}' \n",
                    routingKey, message);
            };
            channle2.BasicConsume(
                queueName2,
                true,
                consumer2
            );
            
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}