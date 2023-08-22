using System.Threading.Tasks;

namespace Estudos.Microsoft.ServiceBus.Consumer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // receive message from the queue
            await MessageScheduledEnqueueTimeTest.ReceiveMessagesAsync();

        }
        
       
    }
}