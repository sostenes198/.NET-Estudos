using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Estudos.Exame.Capitulo1.CreateAndImplementEventsAndCallbacks.Delegates.Action
{
    public class EventHandlerDelegateExceptionStudy
    {
        private static void AlarmListener1(object sender, AlarmEventArgs args)
        {
            Console.WriteLine("Alarm listener 1 called");
            Console.WriteLine(sender);
            Console.WriteLine(args.Location);
            throw new Exception("Bang");
        }

        private static void AlarmListener2(object sender, AlarmEventArgs args)
        {
            Console.WriteLine("Alarm listener 2 called");
            Console.WriteLine(sender);
            Console.WriteLine(args.Location);
            throw new Exception("Boom");
        }

        public static void Run()
        {
            var alarm = new Alarm();
            alarm.OnAlarmRaised += AlarmListener1;
            alarm.OnAlarmRaised += AlarmListener2;

            try
            {
                alarm.RaiseAlarm("Quarto");
            }
            catch (AggregateException ex)
            {
                foreach (var innerException in ex.InnerExceptions)
                {
                    Console.WriteLine(innerException.Message);
                }
            }
        }

        private class Alarm
        {
            public event EventHandler<AlarmEventArgs> OnAlarmRaised = delegate { };

            public void RaiseAlarm(string location)
            {
                var exceptions = new List<Exception>();
                foreach (var handler in OnAlarmRaised.GetInvocationList())
                {
                    try
                    {
                        handler.DynamicInvoke(this, new AlarmEventArgs(location));
                    }
                    catch (TargetInvocationException ex)
                    {
                        exceptions.Add(ex.InnerException);
                    }
                }
                if(exceptions.Any())
                    throw new AggregateException(exceptions);
            }
        }

        private class AlarmEventArgs : EventArgs
        {
            public string Location { get; set; }

            public AlarmEventArgs(string location)
            {
                Location = location;
            }
        }
    }
}