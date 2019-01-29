

// ReSharper disable InconsistentNaming

using Estudos.SSE.Core.Options;
using Estudos.SSE.Tests.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Http;

namespace Estudos.SSE.Tests.Unit.SSE.Options
{
    public class SseMiddlewareOptionsTest
    {
        [Fact(DisplayName = "Deve cria objeto SseMiddlewareOptions")]
        public void ShouldCreateObjectSseMiddlewareOptions()
        {
            // arrange - act
            var result = new SseMiddlewareOptions
            {
                OnPrepareAccept = _ => { }
            };
            
            // assert
            result.OnPrepareAccept.Should().NotBeNull();
        }

        [Fact(DisplayName = "Deve validar propriedades do objeto SseMiddlewareOptions")]
        public void ShouldValidateObjectSseMiddlewareOptionsProperties()
        {
            // arrange
            var expectedProperties = new List<AssertProperty>
            {
                new() {Name = "OnPrepareAccept", Type = typeof(Action<HttpResponse>)}
            };

            // act - assert
            typeof(SseMiddlewareOptions).ValidateProperties(expectedProperties);
        }
    }
}