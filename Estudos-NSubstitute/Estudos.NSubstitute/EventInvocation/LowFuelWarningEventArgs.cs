using System;

namespace Estudos.NSubstitute.EventInvocation
{
    public class LowFuelWarningEventArgs : EventArgs
    {
        public int PercentLeft { get; }
        public LowFuelWarningEventArgs(int percentLeft){
            PercentLeft = percentLeft;
        }
    }
}