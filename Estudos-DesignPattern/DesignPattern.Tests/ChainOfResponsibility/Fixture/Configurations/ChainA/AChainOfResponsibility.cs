using DesignPattern.ChainOfResponsability.Contracts.Base;
using DesignPattern.ChainOfResponsability.Contracts.Base.Step;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainA
{
    internal class AChainOfResponsibility : IChainOfResponsibility
    {
        private readonly IBaseStepChainOfResponsibility _firstStep;

        public AChainOfResponsibility(IBaseStepChainOfResponsibility firstStep) : base(firstStep)
        {
            _firstStep = firstStep;
        }

        public override void Execute()
        {
            _firstStep.Handle();
        }
    }
}