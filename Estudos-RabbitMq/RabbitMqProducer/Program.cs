using RabbitMqProducer.Capitulo_2_Worke_Queues;


namespace RabbitMqProducer
{
    public class Program
    {
        static void Main(string[] args)
        {
            NewTask.SendMessage();
        }
    }
}