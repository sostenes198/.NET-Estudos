using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EstudosPipelinePattern.Main.BlockingCollection.Contracts;

namespace EstudosPipelinePattern.Main.BlockingCollection.PipelinesBuilders
{
    public class CastingPipelineBuilder : IPipeline
    {
        public event Action<object> Finished;

        private List<Func<object, object>> _pipelineSteps = new List<Func<object, object>>();
        private BlockingCollection<object>[] _buffers;

        public void AddStep<TStepIn, TStepOut>(Func<TStepIn, TStepOut> stepFunc)
        {
            _pipelineSteps.Add((objInput) => stepFunc.Invoke((TStepIn)objInput));
        }
        
        public void Execute<TInput>(TInput input)
        {
            var first = _buffers[0];
            first.Add(input);
        }

        public CastingPipelineBuilder GetPipeline()
        {
            _buffers = _pipelineSteps
                .Select(step => new BlockingCollection<object>())
                .ToArray();

            var inputBufferIndex = 0;
            foreach (var pipelineStep  in _pipelineSteps)
            {
                var bufferIndexLocal = inputBufferIndex;
                Task.Run(() =>
                {
                    foreach (var input in _buffers[bufferIndexLocal].GetConsumingEnumerable())
                    {
                        var outPutResult = pipelineStep.Invoke(input);
                        bool isLastStep = bufferIndexLocal == _pipelineSteps.Count - 1;
                        if (isLastStep)
                            Finished?.Invoke(outPutResult);
                        else
                        {
                            var next = _buffers[bufferIndexLocal + 1];
                            next.Add(outPutResult);
                        }
                    }
                });
                inputBufferIndex++;
            }
            return this;
        }

    }
}