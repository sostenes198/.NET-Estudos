using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Estudos.SSE.Core;
using Estudos.SSE.Core.Authorizations.Contracts;
using Estudos.SSE.Core.Clients;
using Estudos.SSE.Core.ClientsIdProviders.Contracts;
using Estudos.SSE.Core.Middlewares;
using Estudos.SSE.Core.Options;
using Estudos.SSE.Core.Services.Contracts;
using Estudos.SSE.Tests.Fixtures;
using Estudos.SSE.Tests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

// ReSharper disable InconsistentNaming
// ReSharper disable StructuredMessageTemplateProblem

namespace Estudos.SSE.Tests.Unit.SSE.Middlewares
{
    [SuppressMessage("Reliability", "CA2017:Incompatibilidade de contagem de parâmetros")]
    public class SseMiddlewareTest
    {
        private const string ClientId = "SseMidleware-UnitTest-1";

        private readonly Mock<ISseService> _sseServiceMock;
        private readonly Mock<IAuthorizationSse> _authorizationMock;
        private readonly Mock<IClientIdProvider> _clientIdProviderMock;
        private readonly Mock<ILogger<SseMiddleware>> _loggerMock;

        private readonly SseMiddleware _sseMiddleware;

        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly HttpContextTest _httpContext;
        private readonly SseClient _sseClient;

        public SseMiddlewareTest()
        {
            _sseServiceMock = new Mock<ISseService>();
            _authorizationMock = new Mock<IAuthorizationSse>();
            _clientIdProviderMock = new Mock<IClientIdProvider>();
            _loggerMock = new Mock<ILogger<SseMiddleware>>();

            var options = new OptionsWrapper<SseMiddlewareOptions>(new SseMiddlewareOptions());

            _sseMiddleware = new SseMiddleware(default!, _sseServiceMock.Object, _authorizationMock.Object, _clientIdProviderMock.Object, options, _loggerMock.Object);

            _httpContext = new HttpContextTest();
            _sseClient = new SseClient(ClientId, _httpContext.Response);

            _cancellationTokenSource = new CancellationTokenSource(1000);
            _httpContext.RequestAborted = _cancellationTokenSource.Token;
        }

        [Fact(DisplayName = "Deve retornar Unauthorized quando requisição não autorizada")]
        public async Task ShouldReturnUnauthorizedWhenNotAuthorizedRequest()
        {
            // arrange
            _authorizationMock.Setup(lnq => lnq.AuthorizeAsync(It.IsAny<HttpContext>())).ReturnsAsync(false);

            // act
            await _sseMiddleware.Invoke(_httpContext);

            // assert
            _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
            ValidateAsserts(_httpContext, 1, 0, 0, 0, 0);
        }

        [Fact(DisplayName = "Deve retornar Conflict quando já existir um cliente conectado com o mesmo clientId")]
        public async Task ShouldReturnConflictWhenExistConnectedClientWithTheSameClientId()
        {
            // arrange
            _authorizationMock.Setup(lnq => lnq.AuthorizeAsync(It.IsAny<HttpContext>())).ReturnsAsync(true);
            _clientIdProviderMock.Setup(lnq => lnq.AcquireClientIdAsync(It.IsAny<HttpContext>())).ReturnsAsync(ClientId);
            _sseServiceMock.Setup(lnq => lnq.IsClientConnectedAsync(It.IsAny<string>())).ReturnsAsync(true);

            // act
            await _sseMiddleware.Invoke(_httpContext);

            // assert
            _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status409Conflict);
            _loggerMock.VerifyLog(lnq => lnq.LogWarning("The IServerSentEventsClient with identifier {ClientId} is already connected. The request can't have been accepted"), Times.Once);
            ValidateAsserts(_httpContext, 1, 1, 1, 0, 0);
        }

        [Fact(DisplayName = "Deve abrir conexão SSE com sucesso")]
        public async Task ShouldOpenSseConnectionSuccessfully()
        {
            // arrange
            _authorizationMock.Setup(lnq => lnq.AuthorizeAsync(It.IsAny<HttpContext>())).ReturnsAsync(true);
            _clientIdProviderMock.Setup(lnq => lnq.AcquireClientIdAsync(It.IsAny<HttpContext>())).ReturnsAsync(ClientId);
            _sseServiceMock.Setup(lnq => lnq.IsClientConnectedAsync(It.IsAny<string>())).ReturnsAsync(false);

            // act
            await _sseMiddleware.Invoke(_httpContext);

            // assert
            _cancellationTokenSource.Token.IsCancellationRequested.Should().BeTrue();

            ((HttpResponseTest) _httpContext.Response).CountOnStarting.Should().Be(1);

            _httpContext.MockResponseBodyFeature.Verify(lnq => lnq.DisableBuffering(), Times.Once);

            _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status200OK);
            _httpContext.Response.ContentType.Should().BeEquivalentTo(SseConstants.SseContentType);
            _httpContext.Response.ContentType.Should().BeEquivalentTo(SseConstants.SseContentType);
            _httpContext.Response.Headers[SseConstants.DisabledBufferingHeader].ToString().Should().BeEquivalentTo(SseConstants.DisabledBuffered);
            _httpContext.Response.Headers[SseConstants.HandledContentResultHeader].ToString().Should().BeEquivalentTo(SseConstants.HandledContentResult);
            ValidateAsserts(_httpContext, 1, 1, 1, 1, 1);
        }

        [Theory(DisplayName = "Deve validar header de resultado")]
        [InlineData("", SseConstants.IdentityContentEncoding)]
        [InlineData("Test-ContentEncondigHeader", "Test-ContentEncondigHeader")]
        public async Task ShouldValidateResponseHeaders(string contentEncodingHeader, string contentEncodingHeaderExpected)
        {
            // arrange - act - assert
            if (!string.IsNullOrEmpty(contentEncodingHeader))
                _httpContext.Response.Headers[SseConstants.ContentEncodingHeader] = contentEncodingHeader;

            var method = typeof(SseMiddleware).GetMethod("ResponseOnStartingCallback", BindingFlags.NonPublic | BindingFlags.Static)!;
            method.Should().NotBeNull();

            var task = (Task) method.Invoke(default, new object[] {_httpContext})!;
            await task;

            var headers = _httpContext.Response.Headers;
            headers[SseConstants.ContentEncodingHeader].ToString().Should().BeEquivalentTo(contentEncodingHeaderExpected);
            headers[SseConstants.CacheControl].ToString().Should().BeEquivalentTo(SseConstants.SseCacheControl);
            headers[SseConstants.Connection].ToString().Should().BeEquivalentTo(SseConstants.SseConnection);
        }

        private void ValidateAsserts(
            HttpContext httpContext,
            int callCountAuthorizeAsync,
            int callCountAcquireClientIdAsync,
            int callCountIsClientConnectedAsync,
            int callCountConnectClientAsync,
            int callCountDisconnectClientAsync)
        {
            _authorizationMock.Verify(lnq => lnq.AuthorizeAsync(httpContext), Times.Exactly(callCountAuthorizeAsync));
            _clientIdProviderMock.Verify(lnq => lnq.AcquireClientIdAsync(httpContext), Times.Exactly(callCountAcquireClientIdAsync));
            _sseServiceMock.Verify(lnq => lnq.IsClientConnectedAsync(ClientId), Times.Exactly(callCountIsClientConnectedAsync));
            _sseServiceMock.Verify(lnq => lnq.ConnectClientAsync(MoqAssert.Assert(_sseClient)), Times.Exactly(callCountConnectClientAsync));
            _sseServiceMock.Verify(lnq => lnq.DisconnectClientAsync(MoqAssert.Assert(_sseClient)), Times.Exactly(callCountDisconnectClientAsync));
        }
    }
}