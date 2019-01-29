using DesignPattern.ChainOfResponsability.Contracts.Base;
using DesignPattern.ChainOfResponsability.Contracts.Base.Step;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainD
{
    public class DChainOfResponsibility: IChainOfResponsibility
    {
        private readonly IBaseStepChainOfResponsibility _firstStep;

        public DChainOfResponsibility(IBaseStepChainOfResponsibility firstStep) : base(firstStep)
        {
            _firstStep = firstStep;
        }

        public override void Execute()
        {
            _firstStep.Handle();
        }
    }
}