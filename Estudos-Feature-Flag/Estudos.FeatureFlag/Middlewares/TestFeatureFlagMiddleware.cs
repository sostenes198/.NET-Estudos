using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Estudos.FeatureFlag.Middlewares
{
    public class TestFeatureFlagMiddleware
    {
        private readonly  RequestDelegate _next;

        public TestFeatureFlagMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task InvokeAsync(HttpContext context)
        {
            throw new Exception("Deu Ruim");
            // await _next.Invoke(context);
        }
    }
}