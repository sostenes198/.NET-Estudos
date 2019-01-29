using System;
using Serilog;
using Serilog.Configuration;

namespace Estudos.Logs.Serilog.Api
{
    public static class StealthConsoleSinkExtensions
    {
        public static LoggerConfiguration StealthConsoleSink(
            this LoggerSinkConfiguration loggerConfiguration,
            string? test = null,
            IFormatProvider? formatProvider = null)
        {
            return loggerConfiguration.Sink(new StealthConsoleSink(formatProvider));
        }
    }
}