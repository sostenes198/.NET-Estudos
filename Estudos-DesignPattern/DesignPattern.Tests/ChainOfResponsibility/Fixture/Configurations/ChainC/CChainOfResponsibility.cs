using DesignPattern.ChainOfResponsability.Contracts.Base;
using DesignPattern.ChainOfResponsability.Contracts.Base.Step;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainC
{
    public class CChainOfResponsibility: IChainOfResponsibility
    {
        private readonly IBaseStepChainOfResponsibility _firstStep;

        public CChainOfResponsibility(IBaseStepChainOfResponsibility firstStep) : base(firstStep)
        {
            _firstStep = firstStep;
        }

        public override void Execute()
        {
            _firstStep.Handle();
        }
    }
}