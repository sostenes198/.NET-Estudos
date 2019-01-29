using DesignPattern.ChainOfResponsability.Contracts.Base.Step;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainA;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Steps
{
    internal class A1 : IStepChainOfResponsibility
    {
        private readonly ARecordStep _recordStep;
        
        private readonly IBaseStepChainOfResponsibility _next;

        public A1(IBaseStepChainOfResponsibility next, ARecordStep recordStep)
        {
            _recordStep = recordStep;
            _next = next;
        }

        public void Handle()
        {
            _recordStep.Records.Add(nameof(A1));
            _next.Handle();
        }
    }
}