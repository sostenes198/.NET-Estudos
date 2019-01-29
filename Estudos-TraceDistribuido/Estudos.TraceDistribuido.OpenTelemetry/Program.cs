// See https://aka.ms/new-console-template for more information

using System.Diagnostics;
using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var serviceName = "Estudos.TraceDistribuido.OpenTelemetry";
var serviceVersion = "1.0.0";

using var traceProvider = Sdk.CreateTracerProviderBuilder()
                             .AddSource(serviceName)
                             .SetResourceBuilder(
                                  ResourceBuilder.CreateDefault()
                                                 .AddService(serviceName, serviceVersion))
                             .AddConsoleExporter()
                             .Build();

var myActivitySource = new ActivitySource(serviceName);
using var activity = myActivitySource.StartActivity("SayHello");
activity?.SetTag("foo", 1);
activity?.SetTag("bar", "Hello, World!");
activity?.SetTag("baz", new[] { 1, 2, 3 });

