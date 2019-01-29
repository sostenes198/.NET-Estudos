

// ReSharper disable InconsistentNaming

using Estudos.SSE.Core.Options;
using Estudos.SSE.Tests.Integration.Collections;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Estudos.SSE.Tests.Integration.SSE.Options
{
    
    [Collection(nameof(SseCollectionTest))]
    public class SseMiddlewareOptionsTest : BaseTest
    {
        private int _countCalledOnPrepareAccept;

        public SseMiddlewareOptionsTest()
        {
            ConfigureServices += (_, collection) => { collection.Configure<SseMiddlewareOptions>(opt => { opt.OnPrepareAccept = _ => _countCalledOnPrepareAccept++; }); };
        }

        [Fact(DisplayName = "Deve validar SseMiddlewareOptions")]
        public void ShouldValidateSseMiddlewareOptions()
        {
            // arrange - act
            var options = ServiceProvider.GetRequiredService<IOptions<SseMiddlewareOptions>>().Value;
            
            options.OnPrepareAccept!.Invoke(default!);
            
            // assert
            _countCalledOnPrepareAccept.Should().Be(1);
        }
    }
}