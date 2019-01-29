using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace Estudos.Colecoes.ThreadSafe.Implementacoes_Thread_Safe.Usar.matrizes.de.coleções.de.bloqueio.em.um.pipeline
{
    public class PipelineFilter<TInput, TOutput>
    {
        private Func<TInput, TOutput> _mProcessor;
        private Action<TInput> _mOutputProcessor;
        private CancellationToken _token;

        public BlockingCollection<TInput>[] MInput;
        public BlockingCollection<TOutput>[] MOutput;
        
        public string Name { get; }

        public PipelineFilter(
            BlockingCollection<TInput>[] input,
            Func<TInput, TOutput> processor,
            CancellationToken token,
            string name)
        {
            MInput = input;
            MOutput = new BlockingCollection<TOutput>[5];
            for (int i = 0; i < MOutput.Length; i++)
                MOutput[i] = new BlockingCollection<TOutput>(500);

            _mProcessor = processor;
            _token = token;
            Name = name;
        }
        
        // Use this constructor for the final endpoint, which does
        // something like write to file or screen, instead of
        // pushing to another pipeline filter.
        public PipelineFilter(
            BlockingCollection<TInput>[] input,
            Action<TInput> renderer,
            CancellationToken token,
            string name)
        {
            MInput = input;
            _mOutputProcessor = renderer;
            _token = token;
            Name = name;
        }
        
        public void Run()
        {
            Console.WriteLine("{0} is running", Name);
            while (!MInput.All(bc => bc.IsCompleted) && !_token.IsCancellationRequested)
            {
                TInput receivedItem;
                int i = BlockingCollection<TInput>.TryTakeFromAny(
                    MInput, out receivedItem, 50, _token);
                if ( i >= 0)
                {
                    if (MOutput != null) // we pass data to another blocking collection
                    {
                        TOutput outputItem = _mProcessor(receivedItem);
                        BlockingCollection<TOutput>.AddToAny(MOutput, outputItem);
                        Console.WriteLine("{0} sent {1} to next", Name, outputItem);
                    }
                    else // we're an endpoint
                    {
                        _mOutputProcessor(receivedItem);
                    }
                }
                else
                {
                    Console.WriteLine("Unable to retrieve data from previous filter");
                }
            }
            if (MOutput != null)
            {
                foreach (var bc in MOutput) bc.CompleteAdding();
            }
        }

    }
}