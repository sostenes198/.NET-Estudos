using System;
using RabbitMqConsumer.Capitulo_2_Worke_Queues;
using RabbitMqConsumer.Capitulo_3_Publish_Subscribe;
using RabbitMqConsumer.Capitulo_4_Routing;
using RabbitMqConsumer.Capitulo_5_Topics;
using RabbitMqConsumer.Capitulo_6_RPC;

namespace RabbitMqConsumer
{
    public class Program
    {
        static void Main(string[] args)
        {
            var rpcClient = new RpcClient();

            Console.WriteLine(" [x] Requesting fib(30)");
            var response = rpcClient.Call("30");

            Console.WriteLine(" [.] Got '{0}'", response);
            rpcClient.Close();
        }
    }
}