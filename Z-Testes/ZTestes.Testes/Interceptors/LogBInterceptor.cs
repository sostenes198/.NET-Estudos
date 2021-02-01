using Castle.DynamicProxy;
using ZTestes.Testes.Interceptors.Base;
using ZTestes.Testes.Log;

namespace ZTestes.Testes.Interceptors
{
    public class LogBInterceptor: Interceptor
    {
        private readonly ILogExample _logger;

        public LogBInterceptor(ILogExample logger)
        {
            _logger = logger;
        }

        protected override void Process(IInvocation invocation)
        {
            _logger.Log($"LogBInterceptor: Método de chamada {invocation.TargetType}.{invocation.Method.Name}.");
            invocation.Proceed();
        }
    }
}