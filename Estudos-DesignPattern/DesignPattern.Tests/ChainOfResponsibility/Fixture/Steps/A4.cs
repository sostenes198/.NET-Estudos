using DesignPattern.ChainOfResponsability.Contracts.Base.Step;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainA;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Steps
{
    internal class A4 : IStepChainOfResponsibility
    {
        private readonly ARecordStep _recordStep;

        private readonly IBaseStepChainOfResponsibility _next;

        public A4(IBaseStepChainOfResponsibility next, ARecordStep recordStep)
        {
            _recordStep = recordStep;
            _next = next;
        }

        public void Handle()
        {
            _recordStep.Records.Add(nameof(A4));
            _next.Handle();
        }
    }
}