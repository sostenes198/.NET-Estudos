using System;
using System.Threading.Tasks;
using MassTransit;

namespace EstudosMassTransit
{
    class Program
    {
        public class Message
        { 
            public string Text { get; set; }
        }
        
        static async Task Main(string[] args)
        {
            var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
            {
                sbc.Host("rabbitmq://localhost");

                sbc.ReceiveEndpoint("test_queue", ep =>
                {
                    ep.Handler<Message>(context =>
                    {
                        return Console.Out.WriteLineAsync($"Received: {context.Message.Text}");
                    });
                });
            });
            
            await bus.StartAsync(); // This is important!

            await bus.Publish(new Message{Text = "Hi"});
        
            Console.WriteLine("Press any key to exit");
            await Task.Run(() => Console.ReadKey());
        
            await bus.StopAsync();
        }
    }
}