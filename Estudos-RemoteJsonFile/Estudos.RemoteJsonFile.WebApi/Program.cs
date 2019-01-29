using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Estudos.RemoteConfigurationProvider;
using Estudos.RemoteConfigurationProvider.Configurations.RemoteFile;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Estudos.RemoteJsonFile.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddRemoteJsonFile(
                        new RemoteFileInfoDefault("https://gitlab.com/sostenes198/remote-appssetings-public/-/raw/master/master.json", false, true));
                })
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
    }
}