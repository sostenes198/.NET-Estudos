using RabbitMqProducer.Capitulo_1_Introducao;
using RabbitMqProducer.Capitulo_2_Worke_Queues;
using RabbitMqProducer.Capitulo_3_Publish_Subscribe;
using RabbitMqProducer.Capitulo_4_Routing;
using RabbitMqProducer.Capitulo_5_Topics;
using RabbitMqProducer.Capitulo_6_RPC;

namespace RabbitMqProducer
{
    public class Program
    {
        static void Main(string[] args)
        {
            Send.SendMessage();
        }
    }
}