using System;
using System.Diagnostics;

namespace Estudos.Exame.Capitulo3.CreateAndMonitorPerformanceCounters
{
    public class Read_To_Event_Log
    {
        public static void Test()
        {
            var categoryName = "Image processing";
            if (EventLog.SourceExists(categoryName) == false)
            {
                Console.WriteLine("Event log not present");
                return;
            }
            
            var imageEventLog = new EventLog
            {
                Source = categoryName
            };
            foreach (EventLogEntry entry in imageEventLog.Entries)
            {
                Console.WriteLine($"Source: {entry.Source} Type: {entry.EntryType} Time: {entry.TimeWritten} Message: {entry.Message}");
            }

        }
    }
}