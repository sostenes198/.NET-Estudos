using DesignPattern.ChainOfResponsability.Contracts.Base;
using DesignPattern.ChainOfResponsability.Contracts.Base.Step;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainB
{
    public class BChainOfResponsibility : IChainOfResponsibility
    {
        private readonly IBaseStepChainOfResponsibility _firstStep;

        public BChainOfResponsibility(IBaseStepChainOfResponsibility firstStep) : base(firstStep)
        {
            _firstStep = firstStep;
        }

        public override void Execute()
        {
            _firstStep.Handle();
        }
    }
}