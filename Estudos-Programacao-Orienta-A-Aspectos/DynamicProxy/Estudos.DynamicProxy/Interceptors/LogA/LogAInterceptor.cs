using System;
using Castle.DynamicProxy;
using Estudos.DynamicProxy.Interceptors.Base;

namespace Estudos.DynamicProxy.Interceptors.LogA
{
    public class LogAInterceptor : GenericInterceptor
    {
        protected override void Process(IInvocation invocation)
        {
            Console.WriteLine("Usando Log A");
            invocation.Proceed(); // continue
        }
    }
}