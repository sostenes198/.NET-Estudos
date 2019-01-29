using System.Reflection;
using Estudos.SSE.Core;
using Estudos.SSE.Core.Extensions;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Estudos.SSE.Tests.Unit.SSE.Extensions
{
    public class HttpContextExtensionsTest
    {
        private readonly CancellationTokenSource _cancellationTokenSource;


        public HttpContextExtensionsTest()
        {
            _cancellationTokenSource = new CancellationTokenSource();
        }

        [Fact(DisplayName = "Deve validar propriedade CompletedResult")]
        public void ShouldValidatePropertyCompletedResult()
        {
            // arrange
            var propertyInfo = typeof(HttpContextExtensions).GetField("CompletedResult", BindingFlags.Static | BindingFlags.NonPublic)!;

            // act
            var completedResult = (bool)propertyInfo.GetValue(default)!;

            // assert
            completedResult.Should().BeTrue();
        }

        [Fact(DisplayName = "Deve validar AcceptAsync")]
        public async Task ShouldValidateAcceptAsync()
        {
            // arrange
            var httpContext = new DefaultHttpContext();

            // act - assert
            await httpContext.Response.Invoking(lnq => lnq.AcceptAsync())
               .Should()
               .NotThrowAsync();

            // assert
            httpContext.Response.ContentType.Should().BeEquivalentTo(SseConstants.SseContentType);
        }

        [Fact(DisplayName = "Deve validar AcceptAsync com onPrepareAccept ")]
        public async Task ShouldValidateAcceptAsyncWithOnPrepareAccept()
        {
            // arrange
            var countCalled = 0;
            Action<HttpResponse> onPrepareAccept = _ => { countCalled++; };
            var httpContext = new DefaultHttpContext();

            // act - assert
            await httpContext.Response.Invoking(lnq => lnq.AcceptAsync(onPrepareAccept))
               .Should()
               .NotThrowAsync();

            // assert
            countCalled.Should().Be(1);
            httpContext.Response.ContentType.Should().BeEquivalentTo(SseConstants.SseContentType);
        }

        [Fact(DisplayName = "Deve validar WaitAsync aguardando token ser cancelado")]
        public async Task ShouldValidateWaitAsyncWaitingTokenBeCanceled()
        {
            // arrange
            var httpContext = new DefaultHttpContext();
            httpContext.RequestAborted = _cancellationTokenSource.Token;

            var timer = new System.Timers.Timer(1000);

            timer.Elapsed += (_, _) =>
            {
                _cancellationTokenSource.Cancel();
            };
            timer.Start();

            // act
            await httpContext.RequestAborted.WaitAsync();

            // assert
            timer.Stop();
        }

        [Fact(DisplayName = "Deve valdiar WaitAsync com token já cancelado")]
        public async Task ShouldValidateWaitAsyncWithCanceledToken()
        {
            // arrange
            var httpContext = new DefaultHttpContext
            {
                RequestAborted = _cancellationTokenSource.Token
            };

            _cancellationTokenSource.Cancel();

            // act - assert
            await httpContext.RequestAborted.WaitAsync();
        }
    }
}