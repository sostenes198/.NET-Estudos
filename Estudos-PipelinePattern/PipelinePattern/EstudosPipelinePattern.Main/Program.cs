using System;
using System.Threading.Tasks.Dataflow;
using EstudosPipelinePattern.Main.TplDataFlow.PipelineBuilders;

namespace EstudosPipelinePattern.Main
{
    class Program
    {
        static void Main(string[] args)
        {
            var pipeline = TplPipeline.CreatePipeline(Console.WriteLine);
            pipeline.Post("The pipeline pattern is the best pattern");

            Console.ReadKey();
        }
    }
}