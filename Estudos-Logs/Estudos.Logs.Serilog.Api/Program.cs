using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;

//using Serilog.Exceptions;

namespace Estudos.Logs.Serilog.Api
{
    public class Program
    {
        public static int Main(string[] args)
        {
            // Log.Logger = new LoggerConfiguration()
            //     .MinimumLevel.Information()
            //     .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            //     .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
            //     .Enrich.FromLogContext()
            //     .Enrich.WithExceptionDetails()
            //     .WriteTo.Seq("http://localhost:5341")
            //     .WriteTo.Console()
            //     .CreateLogger();
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateBootstrapLogger();

            try
            {
                Log.Information("Starting host");
                CreateHostBuilder(args)
                    .Build()
                    .Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
                Host.CreateDefaultBuilder(args)
                    .ConfigureLogging(c => c.ClearProviders())
                    .UseSerilog((context, services, configuration) => configuration
                        .ReadFrom.Configuration(context.Configuration)
                        .ReadFrom.Services(services))
                    .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }