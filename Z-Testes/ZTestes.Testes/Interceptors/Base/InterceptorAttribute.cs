using System;

namespace ZTestes.Testes.Interceptors.Base
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