using System;
using System.Text;
using RabbitMQ.Client;

namespace RabbitMqProducer.Capitulo_1_Introducao
{
    public static class Send
    {
        public static void SendMessage()
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

            var message = "Hello World";
            var body = Encoding.UTF8.GetBytes(message);
            
            channle.BasicPublish(
                "",
                "hello",
                null,
                body
            );
            
            Console.WriteLine(" [x] Sent {0}", message);
            
            Console.WriteLine(" Press [enter] to exit.");
            Console.ReadLine();
        }
    }
}