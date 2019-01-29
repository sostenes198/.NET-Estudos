using System;
using Microsoft.Extensions.Logging;

namespace Estudos.LogsPersonalizado.LogImplentation
{
    public class ColorConsoleLogger : ILogger
    {
        private readonly string _name;
        private readonly Func<ColorConsoleLoggerConfiguration> _getCurrentConfig;
        
        public ColorConsoleLogger(
            string name,
            Func<ColorConsoleLoggerConfiguration> getCurrentConfig) =>
            (_name, _getCurrentConfig) = (name, getCurrentConfig);
        
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (IsEnabled(logLevel))
                return;

            ColorConsoleLoggerConfiguration config = _getCurrentConfig();
            if (config.EventId == 0 || config.EventId == eventId.Id)
            {
                ConsoleColor originalColor = Console.ForegroundColor;
                Console.ForegroundColor = config.LogLevels[logLevel];
                Console.WriteLine($"[{eventId,2}: {logLevel, -12}]");

                Console.ForegroundColor = originalColor;
                Console.WriteLine($"{_name} - {formatter(state, exception)}");
            }
        }

        public bool IsEnabled(LogLevel logLevel) => _getCurrentConfig().LogLevels.ContainsKey(logLevel);

        public IDisposable BeginScope<TState>(TState state) => default;
    }
}