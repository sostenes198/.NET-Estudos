using System;

namespace Estudos.NSubstitute.EventInvocation
{
    public class FuelManagement
    {
        public event EventHandler<LowFuelWarningEventArgs> LowFuelDetected;
        public void DoSomething(){
            LowFuelDetected?.Invoke(this, new LowFuelWarningEventArgs(15));
        }
    }
}