using System;
using Estudos.DynamicProxy.Contracts;
using Estudos.DynamicProxy.Interceptors.Base;
using Estudos.DynamicProxy.Interceptors.LogA;
using Estudos.DynamicProxy.Interceptors.LogC;

namespace Estudos.DynamicProxy.Entities
{
    public class BlogService : IBlogService
    {
        [Interceptor(typeof(LogCInterceptor))]
        public Blog Get(int id)
        {
            return new Blog
            {
                Id = id,
                Title = "test",
                Description = "description",
                CreatedAt = DateTime.Now.Date
            };
        }

        [Interceptor(typeof(LogAInterceptor))]
        public void Save(Blog blog)
        {
        }

        public void Test()
        {
        }
    }
}