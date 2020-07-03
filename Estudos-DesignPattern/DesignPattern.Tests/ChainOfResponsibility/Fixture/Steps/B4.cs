using DesignPattern.ChainOfResponsability.Contracts.Base.Step;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainB;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Steps
{
    internal class B4 : IStepChainOfResponsibility
    {
        private readonly BRecordStep _recordStep;
        private readonly IBaseStepChainOfResponsibility _next;

        public B4(IBaseStepChainOfResponsibility next, BRecordStep recordStep)
        {
            _recordStep = recordStep;
            _next = next;
        }

        public void Handle()
        {
            _recordStep.Records.Add(nameof(B4));
            _next.Handle();
        }
    }
}