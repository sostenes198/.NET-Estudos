using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Moq;
#pragma warning disable CS8618

// ReSharper disable UnassignedGetOnlyAutoProperty

namespace Estudos.SSE.Tests.Fixtures
{
    public class HttpContextTest : HttpContext
    {
        public Mock<IHttpResponseBodyFeature> MockResponseBodyFeature { get; }

        public int CountAbortRequest { get; set; }

        public HttpContextTest()
        {
            MockResponseBodyFeature = new Mock<IHttpResponseBodyFeature>();
            Response = new HttpResponseTest(this);
            Features = new FeatureCollection();
            Features.Set(MockResponseBodyFeature.Object);
        }

        public override void Abort()
        {
            CountAbortRequest++;
        }

        public sealed override IFeatureCollection Features { get; }

        public override HttpRequest Request { get; }

        public override HttpResponse Response { get; }

        public override ConnectionInfo Connection { get; }

        public override WebSocketManager WebSockets { get; }

        public override ClaimsPrincipal User { get; set; }

        public override IDictionary<object, object?> Items { get; set; }

        public override IServiceProvider RequestServices { get; set; }

        public override CancellationToken RequestAborted { get; set; }

        public override string TraceIdentifier { get; set; }

        public override ISession Session { get; set; }
    }

    public sealed class HttpResponseTest : HttpResponse
    {
        public int CountOnStarting { get; set; }

        public HttpResponseTest(HttpContext httpContext)
        {
            HttpContext = httpContext;
            Headers = new HeaderDictionary();
            Body = new StreamTest();
        }

        public override void OnStarting(Func<object, Task>? callback, object state)
        {
            CountOnStarting++;
            callback?.Invoke(state);
        }

        public override void OnCompleted(Func<object, Task>? callback, object state)
        {
            callback?.Invoke(state);
        }

        public override void Redirect(string location, bool permanent)
        {
        }

        public override HttpContext HttpContext { get; }

        public override int StatusCode { get; set; }

        public override IHeaderDictionary Headers { get; }

        public override Stream Body { get; set; }

        public override long? ContentLength { get; set; }

        public override string ContentType { get; set; }

        public override IResponseCookies Cookies { get; }

        public override bool HasStarted { get; }
    }

    public class StreamTest : Stream
    {
        public int CountWriteAsync { get; set; }

        public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            CountWriteAsync++;

            return Task.CompletedTask;
        }

        public override void Flush()
        {
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return 0;
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }

        public override void SetLength(long value)
        {
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
        }

        public override bool CanRead { get; }

        public override bool CanSeek { get; }

        public override bool CanWrite { get; }

        public override long Length { get; }

        public override long Position { get; set; }
    }
}