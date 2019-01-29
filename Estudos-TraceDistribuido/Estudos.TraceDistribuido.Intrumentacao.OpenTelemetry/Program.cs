using System;
using System.Diagnostics;
using System.Threading.Tasks;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace Estudos.TraceDistribuido.Intrumentacao.OpenTelemetry
{
    class Program
    {
        private static readonly ActivitySource Source = new("Estudos.TraceDistribuido.Intrumentacao.OpenTelemetry", "1.0.0");

        static async Task Main(string[] args)
        {
            using var traceProvider = Sdk.CreateTracerProviderBuilder()
                                         .SetResourceBuilder(ResourceBuilder.CreateDefault().AddService("MySample"))
                                         .AddSource("Estudos.TraceDistribuido.Intrumentacao.OpenTelemetry")
                                         .AddConsoleExporter()
                                         .Build();

            await DoSomeWork("banana", 8);
            Console.WriteLine("Example work done");
        }

        // All the functions below simulate doing some arbitrary work
        static async Task DoSomeWork(string foo, int bar)
        {
            using (var activity = Source.StartActivity("SomeWork"))
            {
                activity?.SetTag("foo", foo);
                activity?.SetTag("bar", bar);
                await StepOne();
                activity?.AddEvent(new ActivityEvent("Part way there"));
                await StepTwo();
                activity?.AddEvent(new ActivityEvent("Done now"));
                activity?.SetTag("otel.status_code", "ERROR");
                activity?.SetTag("otel.status_description", "Use this text give more information about the error");
            }
        }

        static async Task StepOne()
        {
            using (Activity activity = Source.StartActivity("StepOne"))
            {
                await Task.Delay(500);
            }
        }

        static async Task StepTwo()
        {
            using (Activity activity = Source.StartActivity("StepTwo"))
            {
                await Task.Delay(1000);
            }
        }
    }
}