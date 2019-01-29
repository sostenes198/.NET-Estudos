using System;

namespace Estudos.NSubstitute.Calculator
{
    public interface ICalculator
    {
        int Add(int a, int b);
        string Mode { get; set; }
        event EventHandler PoweringUp;

        void SayHello(string message);
    }
}