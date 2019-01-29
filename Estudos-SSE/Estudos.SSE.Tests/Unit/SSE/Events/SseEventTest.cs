

// ReSharper disable InconsistentNaming

using Estudos.SSE.Core.Events;
using Estudos.SSE.Tests.Utils;
using FluentAssertions;

namespace Estudos.SSE.Tests.Unit.SSE.Events
{
    public class SseEventTest
    {
        [Fact(DisplayName = "Deve criar objeto SseEvent")]
        public void ShouldCreateObjectSseEvent()
        {
            // arrange - act
            var result = new SseEvent("ID", "TYPE", "DATA");

            // assert
            result.Id.Should().BeEquivalentTo("ID");
            result.Event.Should().BeEquivalentTo("TYPE");
            result.Data.Should().BeEquivalentTo("DATA");
        }

        [Fact(DisplayName = "Deve criar objeto SseEvent com Id nullo")]
        public void ShouldCreateObjectSseEventWithNullId()
        {
            // arrange - act
            var result = new SseEvent("TYPE", "DATA");

            // assert
            result.Id.Should().BeNull();
            result.Event.Should().BeEquivalentTo("TYPE");
            result.Data.Should().BeEquivalentTo("DATA");
        }

        [Fact(DisplayName = "Deve validar propriedades objeto SseEvent")]
        public void ShouldValidateSseEventProperties()
        {
            // arrange
            var expectedProperties = new List<AssertProperty>
            {
                new() {Name = "Id", Type = typeof(string)},
                new() {Name = "Event", Type = typeof(string)},
                new() {Name = "Data", Type = typeof(string)}
            };

            // act - assert
            typeof(SseEvent).ValidateProperties(expectedProperties);
        }
    }
}