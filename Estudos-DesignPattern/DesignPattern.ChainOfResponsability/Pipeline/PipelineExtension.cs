using System;

namespace DesignPattern.ChainOfResponsability.Pipeline
{
    public static class PipelineExtension
    {
        public static IPipeline Pipeline(this object obj)
        {
            return new PipelineImplementation();
        }
        
        public static IPipeline Pipeline<TInput>(this object obj, TInput input)
        {
            return new PipelineImplementation()
                .Pipe(() => input);
        }

        public static void PipelineSetCanceled(this object obj)
        {
            throw new PipelineCanceledException();
        }

        public interface IPipeline
        {
            IPipeline Pipe<Tin, TOut>(Func<Tin, TOut> func);
            IPipeline Pipe<TOut>(Func<TOut> func);
            IPipeline Pipe<TIn>(Action<TIn> func);
            IPipeline Pipe(Action func);
        }

        private class PipelineCanceledException : Exception
        {
        }

        private class PipelineImplementation : IPipeline
        {
            private object Result;
            private bool Canceled;

            public IPipeline Pipe<TIn, TOut>(Func<TIn, TOut> func)
            {
                if (Canceled)
                    return SetCanceled();

                return Execute(func);
            }

            public IPipeline Pipe<TOut>(Func<TOut> func)
            {
                if (Canceled)
                    return SetCanceled();

                return Execute(func);
            }

            public IPipeline Pipe<TIn>(Action<TIn> func)
            {
                if (Canceled)
                    return SetCanceled();

                return Execute(func);
            }

            public IPipeline Pipe(Action func)
            {
                if (Canceled)
                    return SetCanceled();

                return Execute(func);
            }

            private IPipeline Execute<TIn, TOut>(Func<TIn, TOut> func)
            {
                try
                {
                    var result = func(GetResult<TIn>());
                    return SetResult(result);
                }
                catch (PipelineCanceledException)
                {
                    return SetCanceled();
                }
            }

            private IPipeline Execute<TOut>(Func<TOut> func)
            {
                try
                {
                    var result = func();
                    return SetResult(result);
                }
                catch (PipelineCanceledException)
                {
                    return SetCanceled();
                }
            }

            private IPipeline Execute<TIn>(Action<TIn> func)
            {
                try
                {
                    func(GetResult<TIn>());
                    return SetResult();
                }
                catch (PipelineCanceledException)
                {
                    return SetCanceled();
                }
            }

            private IPipeline Execute(Action func)
            {
                try
                {
                    func();
                    return SetResult();
                }
                catch (PipelineCanceledException)
                {
                    return SetCanceled();
                }
            }

            private TIn GetResult<TIn>() => Result == default ? default : (TIn) Result;

            private IPipeline SetResult(object result)
            {
                return new PipelineImplementation
                {
                    Result = result
                };
            }

            private IPipeline SetResult()
            {
                return new PipelineImplementation
                {
                    Result = default
                };
            }

            private IPipeline SetCanceled()
            {
                return new PipelineImplementation
                {
                    Canceled = true
                };
            }
        }
    }
}