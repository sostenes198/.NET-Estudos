using System;

namespace Estudos.Exame.Capitulo1.CreateAndImplementEventsAndCallbacks.Delegates.Action
{
    public class ActionDelegateStudy
    {
        private static void AlarmListener1()
        {
            Console.WriteLine("Alarm listener 1 called");
        }
        
        private static void AlarmListener2()
        {
            Console.WriteLine("Alarm listener 2 called");
        }

        public static void Run()
        {
            var alarm = new Alarm();
            alarm.OnAlarmRaised += AlarmListener1;
            alarm.OnAlarmRaised += AlarmListener2;
            
            alarm.RaiseAlarm();

            alarm.OnAlarmRaised -= AlarmListener2;
            alarm.RaiseAlarm();
        }
        
    }

    public class Alarm
    {
        public event System.Action OnAlarmRaised = delegate {  };

        public void RaiseAlarm()
        {
            OnAlarmRaised?.Invoke();
        }
    }
}