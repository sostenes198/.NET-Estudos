

// ReSharper disable InconsistentNaming

using Estudos.SSE.Core.Events;
using Estudos.SSE.Tests.Utils;
using FluentAssertions;

namespace Estudos.SSE.Tests.Unit.SSE.Events
{
    public class SseEventBytesTest
    {
        [Fact(DisplayName = "Deve criar objeto SseEventBytes")]
        public void ShouldCreateSseEventBytes()
        {
            // arrange 
            var bytes = new byte[]{10,20,30};
            
            // act
            var result = new SseEventBytes(bytes, bytes.Length);
            
            // assert
            result.Bytes.Should().BeEquivalentTo(bytes);
            result.BytesCount.Should().Be(3);
        }

        [Fact(DisplayName = "Deve validar propriedades do objeto SseEventBytes")]
        public void ShouldValidateSseEventBytesProperties()
        {
            // arrange
            var expectedProperties = new List<AssertProperty>
            {
                new() {Name = "Bytes", Type = typeof(byte[])},
                new() {Name = "BytesCount", Type = typeof(int)}
            };

            // act - assert
            typeof(SseEventBytes).ValidateProperties(expectedProperties);
        }
    }
}