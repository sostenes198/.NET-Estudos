using System;
using System.Reflection;
using Castle.DynamicProxy;

namespace Estudos.DynamicProxy.Interceptors.Base
{
    public abstract class Interceptor : IInterceptor
    {
        private readonly Type _typeAttribute;

        protected Interceptor(Type typeAttribute)
        {
            _typeAttribute = typeAttribute;
        }
        
        public void Intercept(IInvocation invocation)
        {
            if (invocation.MethodInvocationTarget.GetCustomAttribute(_typeAttribute) != default || invocation.Method.GetCustomAttribute(_typeAttribute) != default)
            {
                Process(invocation);
                return;
            }
            invocation.Proceed();
        }

        protected abstract void Process(IInvocation invocation);
    }
}