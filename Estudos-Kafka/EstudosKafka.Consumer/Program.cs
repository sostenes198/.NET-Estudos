using System;
using System.Threading;
using Confluent.Kafka;

namespace EstudosKafka.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            string nomeTopico = "estudos_kafka";
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = $"{nomeTopico}-group-0"
            };
            
            var cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };
            
            Console.WriteLine("Pression ctrl+c para sair");
            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(nomeTopico);
            try
            {
                while (true)
                {
                    var mensagem = consumer.Consume(cts.Token);
                    Console.WriteLine($"mensagem lida: {mensagem.Message.Value}");
                }
            }
            catch (Exception)
            {
                consumer.Close();
            }
            
        }
    }
}