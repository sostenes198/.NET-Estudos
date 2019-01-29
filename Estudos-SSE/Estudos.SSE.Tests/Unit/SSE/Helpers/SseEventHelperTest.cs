

// ReSharper disable InconsistentNaming

using Estudos.SSE.Core.Events;
using Estudos.SSE.Core.Helpers;
using FluentAssertions;

namespace Estudos.SSE.Tests.Unit.SSE.Helpers
{
    public class SseEventHelperTest
    {
        [Fact(DisplayName = "Deve validar GetReconnectIntervalBytes")]
        public void ShouldValidateGetReconnectIntervalBytes()
        {
            // arrange - act
            var result = SseEventHelper.GetReconnectIntervalBytes(10);

            // assert
            result.Bytes.Should().BeEquivalentTo(new byte[] {114, 101, 116, 114, 121, 58, 32, 49, 48, 13, 10, 13, 10, 0, 0, 0, 0, 0, 0, 0});
            result.BytesCount.Should().Be(13);
        }

        [Fact(DisplayName = "Deve validar GetCommentBytes")]
        public void ShouldValidateGetCommentBytes()
        {
            // arrange - act
            var result = SseEventHelper.GetCommentBytes("Comment UnitTest");

            // assert
            result.Bytes.Should()
               .BeEquivalentTo(
                    new byte[]
                    {
                        58, 32, 67, 111, 109, 109, 101, 110, 116, 32, 85, 110, 105, 116, 84, 101, 115, 116, 13, 10, 13, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                    });

            result.BytesCount.Should().Be(22);
        }

        [Fact(DisplayName = "Deve validar GetEventBytes com string")]
        public void ShouldValidateGetEventBytesWithString()
        {
            // arrange - act
            var result = SseEventHelper.GetEventBytes("Comment UnitTest");

            // assert
            result.Bytes.Should()
               .BeEquivalentTo(
                    new byte[]
                    {
                        100, 97, 116, 97, 58, 32, 67, 111, 109, 109, 101, 110, 116, 32, 85, 110, 105, 116, 84, 101, 115, 116, 13, 10, 13, 10, 0, 0, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                    });

            result.BytesCount.Should().Be(26);
        }

        [Theory(DisplayName = "Deve validar GetEventBytes com SseEvent")]
        [MemberData(nameof(ScenariosGetEventBytesWithSseEvent))]
        public void ShouldValidateGetEventBytesWithSseEvent(SseEvent sseEvent, Byte[] expectedBytes, int expectedBytesCoun)
        {
            // arrange - act
            var result = SseEventHelper.GetEventBytes(sseEvent);

            // assert
            result.Bytes.Should().BeEquivalentTo(expectedBytes);
            result.BytesCount.Should().Be(expectedBytesCoun);
        }

        public static TheoryData<SseEvent, byte[], int> ScenariosGetEventBytesWithSseEvent =>
            new()
            {
                {
                    new SseEvent("UnitTest", "Unit Test Data"),
                    new byte[]
                    {
                        101, 118, 101, 110, 116, 58, 32, 85, 110, 105, 116, 84, 101, 115, 116, 13, 10, 100, 97, 116, 97, 58, 32, 85, 110, 105, 116, 32, 84, 101, 115, 116, 32, 68, 97, 116, 97, 13, 10, 13, 10, 0, 0, 0, 0,
                        0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                    },
                    41
                },
                {
                    new SseEvent("1", "UnitTest", "Unit Test Data"),
                    new byte[]
                    {
                        105, 100, 58, 32, 49, 13, 10, 101, 118, 101, 110, 116, 58, 32, 85, 110, 105, 116, 84, 101, 115, 116, 13, 10, 100, 97, 116, 97, 58, 32, 85, 110, 105, 116, 32, 84, 101, 115, 116, 32, 68, 97, 116, 97, 13, 10,
                        13, 10, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0
                    },
                    48
                }
            };
    }
}