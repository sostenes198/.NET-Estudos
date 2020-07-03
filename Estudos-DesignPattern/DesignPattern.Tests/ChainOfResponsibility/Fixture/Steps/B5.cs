using DesignPattern.ChainOfResponsability.Contracts.Base.Step;
using DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainB;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Steps
{
    internal class B5 : ILastStepChainOfResponsibility
    {
        private readonly BRecordStep _recordStep;

        public B5(BRecordStep recordStep)
        {
            _recordStep = recordStep;
        }

        public void Handle()
        {
            _recordStep.Records.Add(nameof(B5));
        }
    }
}