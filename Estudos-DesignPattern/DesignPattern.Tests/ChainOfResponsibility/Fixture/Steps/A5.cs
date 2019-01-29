using DesignPattern.ChainOfResponsability.Contracts.Base.Step;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainA;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Steps
{
    internal class A5 : ILastStepChainOfResponsibility
    {
        private readonly ARecordStep _recordStep;

        public A5(ARecordStep recordStep)
        {
            _recordStep = recordStep;
        }

        public void Handle()
        {
            _recordStep.Records.Add(nameof(A5));
        }
    }
}