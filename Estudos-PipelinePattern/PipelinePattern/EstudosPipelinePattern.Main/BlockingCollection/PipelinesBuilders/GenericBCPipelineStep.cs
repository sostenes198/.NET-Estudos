using System;
using System.Collections.Concurrent;
using EstudosPipelinePattern.Main.BlockingCollection.Contracts;

namespace EstudosPipelinePattern.Main.BlockingCollection.PipelinesBuilders
{
    public class GenericBCPipelineStep<TStepIn, TStepOut> : IPipelineStep<TStepIn>
    {
        public BlockingCollection<TStepIn> Buffer { get; set; } = new BlockingCollection<TStepIn>();
        public Func<TStepIn, TStepOut> StepAction { get; set; }
    }
}