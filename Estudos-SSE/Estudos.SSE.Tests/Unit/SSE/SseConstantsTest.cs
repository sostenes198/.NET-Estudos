

// ReSharper disable InconsistentNaming

using Estudos.SSE.Core;
using FluentAssertions;

namespace Estudos.SSE.Tests.Unit.SSE
{
    public class SseConstantsTest
    {
        [Fact(DisplayName = "Deve validar objeto SseConstants")]
        public void ShouldValidateObjectSseConstants()
        {
            // arrange - act - assert
            var expectedProperties = new List<(Type Type, string Name, string Value)>
            {
                (typeof(string), "DisabledBufferingHeader","rx-disabled-bufering"),
                (typeof(string), "DisabledBuffered","true"),
                (typeof(string), "HandledContentResultHeader","rx-handled-content-result"),
                (typeof(string), "HandledContentResult","true"),
                (typeof(string), "ContentEncodingHeader","Content-Encoding"),
                (typeof(string), "IdentityContentEncoding","identity"),
                (typeof(string), "CacheControl","Cache-Control"),
                (typeof(string), "Connection","Connection"),
                (typeof(string), "SseContentType","text/event-stream"),
                (typeof(string), "SseConnection","keep-alive"),
                (typeof(string), "SseCacheControl","no-cache"),
                (typeof(string), "LastEventIdHttpHeader","Last-Event-ID"),
                (typeof(string), "SseRetryField","retry: "),
                (typeof(string), "SseCommentField",": "),
                (typeof(string), "SseIdField","id: "),
                (typeof(string), "SseEventField","event: "),
                (typeof(string), "SseDataField","data: ")
            };

            var fields = typeof(SseConstants).GetFields()
               .Select(lnq => new {Name = lnq.Name, Type = lnq.FieldType, Value = lnq.GetValue(typeof(SseConstants))})
               .ToList();

            var properties = typeof(SseConstants).GetProperties()
               .Select(lnq => new {Name = lnq.Name, Type = lnq.PropertyType, Value = lnq.GetValue(typeof(SseConstants))})
               .ToList();

            foreach (var expectedProperty in expectedProperties)
            {
                var field = fields.FirstOrDefault(lnq => lnq.Name == expectedProperty.Name);

                var property = properties.FirstOrDefault(lnq => lnq.Name == expectedProperty.Name);

                if (field != null)
                {
                    field.Name.Should().BeEquivalentTo(expectedProperty.Name);
                    field.Type.Should().Be(expectedProperty.Type);
                    fields.Remove(field);
                }

                if (property != null)
                {
                    property.Name.Should().BeEquivalentTo(expectedProperty.Name);
                    property.Type.Should().Be(expectedProperty.Type);
                    properties.Remove(property);
                }
            }

            fields.Count.Should().Be(0);
            properties.Count.Should().Be(0);
        }
    }
}