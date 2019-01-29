using Jaeger;
using Jaeger.Samplers;
using Jaeger.Senders;
using Jaeger.Senders.Thrift;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OpenTracing;
using OpenTracing.Util;

namespace Common.Tracing.Jaeger;
public static class Extension
{
    public static IServiceCollection AddJaegerEnv(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<ITracer>(serviceProvider =>
        {
            var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

            /*                                    Utilizando ENV                                                               */
            // var senderConfig = new Configuration.SenderConfiguration(loggerFactory)
            //    .WithAgentHost(Environment.GetEnvironmentVariable("JAEGER_AGENT_HOST"))
            //    .WithAgentPort(Convert.ToInt32(Environment.GetEnvironmentVariable("JAEGER_AGENT_PORT")));
            //
            // Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
            //    .RegisterSenderFactory<ThriftSenderFactory>();
            //
            // var config = Configuration.FromIConfiguration(loggerFactory, configuration);
            
            var senderConfig = new Configuration.SenderConfiguration(loggerFactory)
               .WithAgentHost(configuration["JAEGER_AGENT_HOST"])
               .WithAgentPort(Convert.ToInt32(configuration["JAEGER_AGENT_PORT"]));

            Configuration.SenderConfiguration.DefaultSenderResolver = new SenderResolver(loggerFactory)
               .RegisterSenderFactory<ThriftSenderFactory>();

            var config = Configuration.FromIConfiguration(loggerFactory, configuration);

            var samplerConfiguration = new Configuration.SamplerConfiguration(loggerFactory)
               .WithType(ConstSampler.Type)
               .WithParam(1);

            var reporterConfiguration = new Configuration.ReporterConfiguration(loggerFactory)
               .WithSender(senderConfig)
               .WithLogSpans(true);

            var tracer = config
               .WithSampler(samplerConfiguration)
               .WithReporter(reporterConfiguration)
               .GetTracer();

            GlobalTracer.Register(tracer);

            return tracer;
        });

        return services;
    }
}