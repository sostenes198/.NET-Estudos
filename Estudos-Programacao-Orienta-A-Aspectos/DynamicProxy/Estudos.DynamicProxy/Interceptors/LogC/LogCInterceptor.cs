using System;
using Castle.DynamicProxy;
using Estudos.DynamicProxy.Interceptors.Base;

namespace Estudos.DynamicProxy.Interceptors.LogC
{
    public class LogCInterceptor: GenericInterceptor
    {
        protected override void Process(IInvocation invocation)
        {
            Console.WriteLine("Usando Log C");
            invocation.Proceed(); // continue
        }
    }
}