// Define some important constants to initialize tracing with

using System.Diagnostics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var serviceName = "Estudos.TraceDistribuido.OpenTelemetry.AspNetCore";
var serviceVersion = "1.0.0";

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenTelemetryTracing(providerBuilder =>
{
    providerBuilder
       .AddConsoleExporter()
       .AddSource(serviceName)
       .SetResourceBuilder(
            ResourceBuilder.CreateDefault()
                           .AddService(serviceName, serviceVersion))
       .AddHttpClientInstrumentation()
       .AddAspNetCoreInstrumentation()
       .AddSqlClientInstrumentation();
});

var myActivitySource = new ActivitySource(serviceName);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

var httpClient = new HttpClient();

app.MapGet("/hello", async () =>
{
    // Track work inside of the request
    using var activity = myActivitySource.StartActivity("SayHello");
    activity?.SetTag("foo", 1);
    activity?.SetTag("bar", "Hello, World!");
    activity?.SetTag("baz", new int[] {1, 2, 3});

    var html = await httpClient.GetStringAsync("https://www.example.com/");

    return "Hello, World!";
});

app.Run();