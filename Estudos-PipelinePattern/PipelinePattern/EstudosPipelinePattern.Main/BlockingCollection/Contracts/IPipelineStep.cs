using System.Collections.Concurrent;

namespace EstudosPipelinePattern.Main.BlockingCollection.Contracts
{
    public interface IPipelineStep<StepIn>
    {
        BlockingCollection<StepIn> Buffer { get; set; }
    }
}