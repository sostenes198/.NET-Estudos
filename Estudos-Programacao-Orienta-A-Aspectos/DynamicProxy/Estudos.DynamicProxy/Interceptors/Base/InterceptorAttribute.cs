using System;

namespace Estudos.DynamicProxy.Interceptors.Base
{
    [AttributeUsage(AttributeTargets.Method)]
    public class InterceptorAttribute : Attribute
    {
        public Type Interceptor { get; }

        public InterceptorAttribute(Type interceptor)
        {
            Interceptor = interceptor;
        }
    }
}