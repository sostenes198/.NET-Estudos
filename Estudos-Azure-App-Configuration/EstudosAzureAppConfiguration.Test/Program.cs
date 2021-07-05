using System;
using Microsoft.Extensions.Configuration;

namespace EstudosAzureAppConfiguration.Test
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder();
            builder.AddAzureAppConfiguration(config =>
            {
                config.Connect(Environment.GetEnvironmentVariable("ConnectionString_Estudos_AppConfiguration"))
                    .ConfigureRefresh(configRefresh =>
                    {
                        configRefresh.Register("TestApp:Settings:Sentinel", true);
                        configRefresh.SetCacheExpiration(new TimeSpan(0, 0, 5));
                    });
            });
            var config = builder.Build();

            while (true)
            {
                Console.WriteLine(config["TestApp:Settings:Message"] ?? "Sem nenhuma variável configurada");

                Console.WriteLine("Pressione qualquer tecla para verificar novamente");
                Console.ReadKey();
            }
        }
    }
}