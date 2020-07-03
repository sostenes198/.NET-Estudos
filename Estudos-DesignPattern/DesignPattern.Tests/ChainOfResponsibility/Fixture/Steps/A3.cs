using DesignPattern.ChainOfResponsability.Contracts.Base.Step;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainA;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Steps
{
    internal class A3 : IStepChainOfResponsibility
    {
        private readonly ARecordStep _recordStep;
        private readonly IBaseStepChainOfResponsibility _next;

        public A3(IBaseStepChainOfResponsibility next, ARecordStep recordStep)
        {
            _recordStep = recordStep;
            _next = next;
        }

        public void Handle()
        {
            _recordStep.Records.Add(nameof(A3));
            _next.Handle();
        }
    }
}