using System;
using EstudosPipelinePattern.Main.BlockingCollection.PipelinesBuilders;

namespace EstudosPipelinePattern.Main.BlockingCollection.Extensions
{
    public static class GenericBCPipelineExtensions
    {
        public static TOutPut Step<TInput, TOutPut, TInputOuter, TOutPutOuter>(this TInput inputType,
            GenericBCPipeline<TInputOuter, TOutPutOuter> pipelineBuilder,
            Func<TInput, TOutPut> step)
        {
            var pipelineStep = pipelineBuilder.GenerateStep<TInput, TOutPut>();
            pipelineStep.StepAction = step;
            return default(TOutPut);
        }
    }
}