using System;
using Microsoft.Azure.WebJobs;
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