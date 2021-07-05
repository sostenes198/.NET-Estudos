using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Estudos.FeatureFlag
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder
                        .ConfigureAppConfiguration(config =>
                        {
                            var settings = config.Build();
                            config.AddAzureAppConfiguration(options =>
                                options.Connect(settings["ConnectionStrings:AppConfig"]).UseFeatureFlags(flagOptions =>
                                {
                                    flagOptions.CacheExpirationInterval = TimeSpan.FromSeconds(30);
                                }));
                        })
                        .UseStartup<Startup>();
                });
    }
}