using System.Threading.Tasks;

namespace Estudos.Microsoft.ServiceBus.Sender
{
    class Program
    {
        static async Task  Main(string[] args)
        {
            await MessageScheduledEnqueueTimeTest.SendMessageAsync();
            // await Message.SendMessageBatchAsync();
            // await Topic.SendMessageToTopicAsync();
            // await Topic.SendMessageBatchToTopicAsync();
        }
        
      
    }
}