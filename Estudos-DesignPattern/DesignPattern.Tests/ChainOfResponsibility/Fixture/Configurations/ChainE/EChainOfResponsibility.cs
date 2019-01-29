using DesignPattern.ChainOfResponsability.Contracts.Base;
using DesignPattern.ChainOfResponsability.Contracts.Base.Step;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainE
{
    public class EChainOfResponsibility: IChainOfResponsibility
    {
        private readonly IBaseStepChainOfResponsibility _firstStep;

        public EChainOfResponsibility(IBaseStepChainOfResponsibility firstStep) : base(firstStep)
        {
            _firstStep = firstStep;
        }

        public override void Execute()
        {
            _firstStep.Handle();
        }
    }
}