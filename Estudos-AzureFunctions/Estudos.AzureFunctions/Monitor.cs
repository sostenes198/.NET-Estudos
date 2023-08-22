using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;

namespace Estudos.AzureFunctions
{
    public static class Monitor
    {
        [FunctionName("E3_Monitor")]
        public static async Task RunMonitorAsync([OrchestrationTrigger] IDurableOrchestrationContext context, ILogger log)
        {
            var input = context.GetInput<MonitorRequest>();
            if (!context.IsReplaying) log.LogInformation($"Received monitor request. Location: {input?.Location}. Phone: {input?.Phone}.");

            VerifyRequest(input);

            DateTime endTime = context.CurrentUtcDateTime.AddHours(6);
            if (!context.IsReplaying) log.LogInformation($"Instantiating monitor for {input.Location}. Expires: {endTime}.");

            while (context.CurrentUtcDateTime < endTime)
            {
                // Check the weather
                if (!context.IsReplaying) log.LogInformation($"Checking current weather conditions for {input.Location} at {context.CurrentUtcDateTime}.");

                bool isClear = await context.CallActivityAsync<bool>("E3_GetIsClear", input.Location);

                if (isClear)
                {
                    // It's not raining! Or snowing. Or misting. Tell our user to take advantage of it.
                    if (!context.IsReplaying)
                    {
                        log.LogInformation($"Detected clear weather for {input.Location}. Notifying {input.Phone}.");
                    }

                    await context.CallActivityAsync("E3_SendGoodWeatherAlert", input.Phone);
                    break;
                }

                // Wait for the next checkpoint
                var nextCheckpoint = context.CurrentUtcDateTime.AddMinutes(2);
                if (!context.IsReplaying) log.LogInformation($"Next check for {input.Location} at {nextCheckpoint}.");

                await context.CreateTimer(nextCheckpoint, CancellationToken.None);
            }

            log.LogInformation($"Monitor expiring.");
        }

        [FunctionName("E3_GetIsClear")]
        public static bool GetIsClear([ActivityTrigger] Location location)
        {
            return true; //Convert.ToBoolean(new Random().Next(0, 1));
        }

        // [FunctionName("E3_SendGoodWeatherAlert")]
        // public static void SendGoodWeatherAlert(
        //     [ActivityTrigger] string phoneNumber,
        //     ILogger log,
        //     [TwilioSms(AccountSidSetting = "TwilioAccountSid", AuthTokenSetting = "TwilioAuthToken", From = "%TwilioPhoneNumber%")]
        //     out CreateMessageOptions message)
        // {
        //     message = new CreateMessageOptions(new PhoneNumber(phoneNumber))
        //     {
        //         Body = @"
        //                 Menino Ed !!!!
        //                 Soso te amaaaaa !!!! <3 <3 
        //                 Você é o irmaozão dele <3 <3 
        //         "
        //     };
        // }

        [Deterministic]
        private static void VerifyRequest(MonitorRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request), "An input object is required.");
            }

            if (request.Location == null)
            {
                throw new ArgumentNullException(nameof(request.Location), "A location input is required.");
            }

            if (string.IsNullOrEmpty(request.Phone))
            {
                throw new ArgumentNullException(nameof(request.Phone), "A phone number input is required.");
            }
        }
    }

    public class MonitorRequest
    {
        public Location Location { get; set; }

        public string Phone { get; set; }
    }

    public class Location
    {
        public string State { get; set; }

        public string City { get; set; }

        public override string ToString() => $"{City}, {State}";
    }
}