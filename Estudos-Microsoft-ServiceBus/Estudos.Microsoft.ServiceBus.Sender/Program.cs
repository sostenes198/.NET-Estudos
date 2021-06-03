using System.Threading.Tasks;

namespace Estudos.Microsoft.ServiceBus.Sender
{
    class Program
    {
        static async Task  Main(string[] args)
        {
            await Message.SendMessageAsync();
            // await Message.SendMessageBatchAsync();
            // await Topic.SendMessageToTopicAsync();
            // await Topic.SendMessageBatchToTopicAsync();
        }
        
      
    }
}