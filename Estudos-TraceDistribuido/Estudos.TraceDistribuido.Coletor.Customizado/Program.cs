using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Estudos.TraceDistribuido.Coletor.Customizado
{
    class Program
    {
        private static readonly ActivitySource Source = new ActivitySource("Sample.DistributedTracing");

        static async Task Main(string[] args)
        {
            Activity.DefaultIdFormat = ActivityIdFormat.W3C;
            Activity.ForceDefaultIdFormat = true;
            Console.WriteLine("         {0,-15} {1,-60} {2,-15}", "OperationName", "Id", "Duration");
            ActivitySource.AddActivityListener(new ActivityListener()
            {
                ShouldListenTo = source => true,
                Sample = (ref ActivityCreationOptions<ActivityContext> options) => ActivitySamplingResult.AllDataAndRecorded,
                ActivityStarted = activity => Console.WriteLine("Started: {0,-15} {1,-60}", activity.OperationName, activity.Id),
                ActivityStopped = activity => Console.WriteLine("Stopped: {0,-15} {1,-60} {2,-15}", activity.OperationName, activity.Id, activity.Duration)
            });

            await DoSomeWork();
            Console.WriteLine("Example work done");
        }

        static async Task DoSomeWork()
        {
            using (Activity a = Source.StartActivity("SomeWork"))
            {
                await StepOne();
                await StepTwo();
            }
        }

        static async Task StepOne()
        {
            using (Activity a = Source.StartActivity("StepOne"))
            {
                await Task.Delay(500);
            }
        }

        static async Task StepTwo()
        {
            using (Activity a = Source.StartActivity("StepTwo"))
            {
                await Task.Delay(1000);
            }
        }
    }
}