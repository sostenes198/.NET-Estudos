using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Estudos.LogsPersonalizado.LogImplentation
{
    public class ColorConsoleLoggerConfiguration
    {
        public int EventId { get; set; }

        public Dictionary<LogLevel, ConsoleColor> LogLevels { get; set; } = new Dictionary<LogLevel, ConsoleColor>
        {
            [LogLevel.Information] = ConsoleColor.Green
        };
    }
}