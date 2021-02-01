using System.Reflection;
using Castle.DynamicProxy;

namespace ZTestes.Testes.Interceptors.Base
{
    public abstract class Interceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            if (GetInterceptorName(invocation) == GetType().Name)
            {
                Process(invocation);
                return;
            }
            invocation.Proceed();
        }
        
        private string GetInterceptorName(IInvocation invocation)
        {
            if (invocation.MethodInvocationTarget.GetCustomAttribute(typeof(InterceptorAttribute)) is InterceptorAttribute methodInvocationTargetInterceptorAttribute)
                return methodInvocationTargetInterceptorAttribute.Interceptor.Name;
            
            if (invocation.Method.GetCustomAttribute(typeof(InterceptorAttribute)) is InterceptorAttribute methodInterceptorAttribute)
                return methodInterceptorAttribute.Interceptor.Name;

            return string.Empty;
        }

        protected abstract void Process(IInvocation invocation);
    }
}