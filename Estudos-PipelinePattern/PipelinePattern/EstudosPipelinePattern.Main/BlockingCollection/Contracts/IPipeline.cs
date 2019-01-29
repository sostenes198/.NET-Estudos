using System;

namespace EstudosPipelinePattern.Main.BlockingCollection.Contracts
{
    public interface IPipeline
    {
        void Execute<TInput>(TInput input);
        event Action<object> Finished;
    }
}