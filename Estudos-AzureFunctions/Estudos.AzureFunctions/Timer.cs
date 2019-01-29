using System;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace Estudos.AzureFunctions
{
    public static class Timer
    {
        [FunctionName("Timer")]
        public static void Run([TimerTrigger("*/10 * * * * *")] TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.UtcNow}");
        }
    }
}