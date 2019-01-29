using DesignPattern.ChainOfResponsability.Contracts.Base.Step;

namespace DesignPattern.ChainOfResponsability.Contracts.Base
{
    public abstract class IChainOfResponsibility
    {
        private IBaseStepChainOfResponsibility FirstStep { get; }

        protected IChainOfResponsibility(IBaseStepChainOfResponsibility firstStep)
        {
            FirstStep = firstStep;
        }
        
        public abstract void Execute();
    }
}