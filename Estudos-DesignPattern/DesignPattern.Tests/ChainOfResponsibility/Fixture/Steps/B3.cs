using DesignPattern.ChainOfResponsability.Contracts.Base.Step;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainB;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Steps
{
    internal class B3 : IStepChainOfResponsibility
    {
        private readonly BRecordStep _recordStep;
        private readonly IBaseStepChainOfResponsibility _next;

        public B3(IBaseStepChainOfResponsibility next, BRecordStep recordStep)
        {
            _recordStep = recordStep;
            _next = next;
        }

        public void Handle()
        {
            _recordStep.Records.Add(nameof(B3));
            _next.Handle();
        }
    }
}