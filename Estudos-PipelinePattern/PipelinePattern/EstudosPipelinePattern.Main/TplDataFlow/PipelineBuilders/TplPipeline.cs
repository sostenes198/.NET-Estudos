using System;
using System.Linq;
using System.Threading.Tasks.Dataflow;

namespace EstudosPipelinePattern.Main.TplDataFlow.PipelineBuilders
{
    public class TplPipeline
    {
        public static TransformBlock<string, string> CreatePipeline(Action<bool> resultCallback)
        {
            var step1 = new TransformBlock<string, string>(FindMostCommon, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 3,
                BoundedCapacity = 5
            });
            var step2 = new TransformBlock<string, int>(word => word.Length, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 1,
                BoundedCapacity = 13
            });
            var step3 = new TransformBlock<int, bool>(lenght => lenght % 2 == 1, new ExecutionDataflowBlockOptions
            {
                MaxDegreeOfParallelism = 11,
                BoundedCapacity = 6
            });
            var callBackStep = new ActionBlock<bool>(resultCallback);
            step1.LinkTo(step2, new DataflowLinkOptions());
            step2.LinkTo(step3, new DataflowLinkOptions());
            step3.LinkTo(callBackStep);
            
            return step1;
        }
        
        private static string FindMostCommon(string input)
        {
            return input.Split(' ')
                .GroupBy(word => word)
                .OrderBy(group => group.Count())
                .Last()
                .Key;
        }
    }
}