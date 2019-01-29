using System.Diagnostics;

namespace Estudos.Exame.Capitulo3.CreateAndMonitorPerformanceCounters
{
    public class Write_To_Event_Log
    {
        private static EventLog _imageEventLog;

        public static void Test()
        {
            SetupLog();
            _imageEventLog.WriteEntry("Processing started");
            _imageEventLog.WriteEntry("Image processing ended");
        }

        private static void SetupLog()
        {
            var categoryName = "Image processing";
            if (EventLog.SourceExists(categoryName) == false)
                EventLog.CreateEventSource(categoryName, $"{categoryName} log");

            _imageEventLog = new EventLog {Source = categoryName};
        }
    }
}