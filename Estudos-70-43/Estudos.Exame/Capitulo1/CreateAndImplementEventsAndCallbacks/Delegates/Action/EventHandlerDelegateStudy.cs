using System;

namespace Estudos.Exame.Capitulo1.CreateAndImplementEventsAndCallbacks.Delegates.Action
{
    public class EventHandlerDelegateStudy
    {
        private static void AlarmListener1(object sender, AlarmEventArgs args)
        {
            Console.WriteLine("Alarm listener 1 called");
            Console.WriteLine(sender);
            Console.WriteLine(args.Location); 
        }
        
        private static void AlarmListener2(object sender, AlarmEventArgs args)
        {
            Console.WriteLine("Alarm listener 2 called");
            Console.WriteLine(sender);
            Console.WriteLine(args.Location);
        }
        
        public static void Run()
        {
            var alarm = new Alarm();
            alarm.OnAlarmRaised += AlarmListener1;
            alarm.OnAlarmRaised += AlarmListener2;
            
            alarm.RaiseAlarm("First Location");

            alarm.OnAlarmRaised -= AlarmListener2;
            alarm.RaiseAlarm("Second Location");
        }
        
        private class Alarm
        {
            public event EventHandler<AlarmEventArgs> OnAlarmRaised = delegate { };

            public void RaiseAlarm(string location)
            {
                OnAlarmRaised(this, new AlarmEventArgs(location));
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