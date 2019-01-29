using System;
using Serilog.Core;
using Serilog.Events;

namespace Estudos.Logs.Serilog.Api
{
    public class StealthConsoleSink: ILogEventSink
    {
        readonly IFormatProvider _formatProvider;
 
        public StealthConsoleSink(IFormatProvider formatProvider)
        {
            _formatProvider = formatProvider;
        }
 
        public void Emit(LogEvent logEvent)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.WriteLine(logEvent.RenderMessage(_formatProvider));
            Console.ResetColor();
        }
    }
}