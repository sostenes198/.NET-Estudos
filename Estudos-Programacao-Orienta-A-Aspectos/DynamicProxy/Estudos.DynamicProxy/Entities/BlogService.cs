using System;
using Estudos.DynamicProxy.Contracts;
using Estudos.DynamicProxy.Interceptors.Base;
using Estudos.DynamicProxy.Interceptors.Log;
using Estudos.DynamicProxy.Interceptors.LogA;
using Estudos.DynamicProxy.Interceptors.LogB;

namespace Estudos.DynamicProxy.Entities
{
    public class BlogService : IBlogService
    {
        [LogInterceptor]
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