using Estudos.DynamicProxy.Entities;
using Estudos.DynamicProxy.Interceptors.Base;
using Estudos.DynamicProxy.Interceptors.Log;
using Estudos.DynamicProxy.Interceptors.LogA;
using Estudos.DynamicProxy.Interceptors.LogB;

namespace Estudos.DynamicProxy.Contracts
{
    public interface IBlogService
    {
        Blog Get(int id);
        void Save(Blog blog);
        
        [Interceptor(typeof(LogBInterceptor))]
        void Test();
    }
}