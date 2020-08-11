using System;
using System.Text;
using System.Threading;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace RabbitMqConsumer.Capitulo_2_Worke_Queues
{
    public class Worker
    {
        public static void RecieveMessage()
        {
            var factory = new ConnectionFactory {HostName = "localhost"};
            using var connection = factory.CreateConnection();
            using var channle = connection.CreateModel();
            channle.QueueDeclare(
                "task_queue",
                true,
                false,
                false,
                null
            );

            var consumer = new EventingBasicConsumer(channle);
            consumer.Received += (sender, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received {0}", message);

                int dots = message.Split('.').Length - 1;
                Thread.Sleep(dots * 1000);

                Console.WriteLine(" [x] Done");

                //channle.BasicAck(ea.DeliveryTag, false);
                ((EventingBasicConsumer) sender)?.Model.BasicAck(ea.DeliveryTag, false);
            };
            channle.BasicConsume(
                "task_queue",
                false,
                consumer
            );

            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}