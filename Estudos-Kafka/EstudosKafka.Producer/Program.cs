using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace EstudosKafka.Producer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string nomeTopico = "estudos_kafka";
            var config = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };
            
            Console.WriteLine("Digite sair para sair");
            using var producer = new ProducerBuilder<Null, string>(config).Build();
            while (true)
            {
                Console.Write("Escreva sua mensagem: ");
                var mensagem = Console.ReadLine();
                if (mensagem == "sair")
                    break;
                await producer.ProduceAsync(nomeTopico, new Message<Null, string> {Value = mensagem});
            }
        }
    }
}