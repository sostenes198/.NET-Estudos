using DesignPattern.ChainOfResponsability.Contracts.Base;
using DesignPattern.ChainOfResponsability.Contracts.Base.Step;

namespace DesignPatternTests.ChainOfResponsibility.Fixture.Configurations.ChainA
{
    public abstract class IAChainOfResponsibility : IChainOfResponsibility
    {
        protected IAChainOfResponsibility(IBaseStepChainOfResponsibility firstStep) : base(firstStep)
        {
        }
    }
}