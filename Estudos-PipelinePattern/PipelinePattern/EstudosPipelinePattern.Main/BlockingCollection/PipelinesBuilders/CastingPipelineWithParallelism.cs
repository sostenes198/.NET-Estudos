using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstudosPipelinePattern.Main.BlockingCollection.Contracts;

namespace EstudosPipelinePattern.Main.BlockingCollection.PipelinesBuilders
{
    public class CastingPipelineWithParallelism : IPipeline
    {
        class Step
        {
            public Func<object, object> Func { get; set; }
            public int DegreeOfParallelism { get; set; }
            public int MaxCapacity { get; set; }
        }

        List<Step> _pipelineSteps = new List<Step>();
        BlockingCollection<object>[] _buffers;
        public event Action<object> Finished;

        public void AddStep(Func<object, object> stepFunc, int degreeOfParallelism, int maxCapacity)
        {
            _pipelineSteps.Add(new Step
            {
                Func = stepFunc,
                DegreeOfParallelism = degreeOfParallelism,
                MaxCapacity = maxCapacity
            });
        }

        public void Execute<TInput>(TInput input)
        {
            var first = _buffers[0];
            first.Add(input);
        }

        public IPipeline GetPipeline()
        {
            _buffers = _pipelineSteps.Select(step => new BlockingCollection<object>(step.MaxCapacity)).ToArray();
            var bufferIndex = 0;
            foreach (var pipelineStep in _pipelineSteps)
            {
                var bufferIndexLocal = bufferIndex;
                for (int i = 0; i < pipelineStep.DegreeOfParallelism; i++)
                {
                    Task.Run(() => StartStep(bufferIndexLocal, pipelineStep));
                }

                bufferIndex++;
            }

            return this;
        }

        private void StartStep(int bufferIndexLocal, Step pipelineStep)
        {
            foreach (var input in _buffers[bufferIndexLocal].GetConsumingEnumerable())
            {
                var outPut = pipelineStep.Func.Invoke(input);
                bool isLastStep = bufferIndexLocal == _pipelineSteps.Count - 1;
                if(isLastStep)
                    Finished?.Invoke(outPut);
                else
                {
                    var next = _buffers[bufferIndexLocal + 1];
                    next.Add(outPut);
                }
            }
        }
    }
}