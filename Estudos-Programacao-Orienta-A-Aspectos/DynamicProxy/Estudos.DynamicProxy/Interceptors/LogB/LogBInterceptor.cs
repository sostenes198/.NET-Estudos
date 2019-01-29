using System;
using Castle.DynamicProxy;
using Estudos.DynamicProxy.Interceptors.Base;

namespace Estudos.DynamicProxy.Interceptors.LogB
{
    public class LogBInterceptor : GenericInterceptor
    {
        protected override void Process(IInvocation invocation)
        {
            Console.WriteLine("Usando Log B");
            invocation.Proceed(); // continue
        }
    }
}