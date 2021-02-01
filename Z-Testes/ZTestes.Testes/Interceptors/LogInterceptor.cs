using Castle.DynamicProxy;
using ZTestes.Testes.Interceptors.Base;
using ZTestes.Testes.Log;

namespace ZTestes.Testes.Interceptors
{
    public class LogInterceptor : Interceptor
    {
        private readonly ILogExample _logger;

        public LogInterceptor(ILogExample logger)
        {
            _logger = logger;
        }

        protected override void Process(IInvocation invocation)
        {
            _logger.Log($"LogInterceptor: Método de chamada {invocation.TargetType}.{invocation.Method.Name}.");
            invocation.Proceed();
        }
    }
}