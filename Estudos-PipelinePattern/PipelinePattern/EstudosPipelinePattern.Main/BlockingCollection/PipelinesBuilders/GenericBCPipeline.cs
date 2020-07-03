using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EstudosPipelinePattern.Main.BlockingCollection.Contracts;

namespace EstudosPipelinePattern.Main.BlockingCollection.PipelinesBuilders
{
    public class GenericBCPipeline<TPipeIn, TPipeOut>
    {
        private List<object> _pipelineSteps = new List<object>();
    
        public event Action<TPipeOut> Finished;

        public GenericBCPipeline(Func<TPipeIn, GenericBCPipeline<TPipeIn, TPipeOut>, TPipeOut> steps)
        {
            steps.Invoke(default(TPipeIn), this);
        }

        public void Execute(TPipeIn input)
        {
            var first = _pipelineSteps[0] as IPipelineStep<TPipeIn>;
            first.Buffer.Add(input);
        }

        public GenericBCPipelineStep<TStepIn, TStepOut> GenerateStep<TStepIn, TStepOut>()
        {
            var pipelineStep = new GenericBCPipelineStep<TStepIn, TStepOut>();
            var stepIndex = _pipelineSteps.Count;

            Task.Run(() =>
            {
                IPipelineStep<TStepOut> nextPipelineStep = default;

                foreach (var input in pipelineStep.Buffer.GetConsumingEnumerable())
                {
                    bool isLastStep = stepIndex == _pipelineSteps.Count - 1;
                    var outPut = pipelineStep.StepAction(input);
                    if(isLastStep)
                        Finished?.Invoke((TPipeOut)(object)outPut);
                    else
                    {
                        nextPipelineStep ??= _pipelineSteps[stepIndex + 1] as IPipelineStep<TStepOut>;
                        nextPipelineStep.Buffer.Add(outPut);
                    }
                }
            });
            _pipelineSteps.Add(pipelineStep);
            return pipelineStep;
        }
    }
}