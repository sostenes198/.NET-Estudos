// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Core.Events
{
    public sealed class SseEvent
    {
        public string? Id { get; }

        public string Event { get; }

        public string Data { get; }

        public SseEvent(in string @event, in string data)
            : this(null, @event, data)
        {
        }

        public SseEvent(in string? id, in string @event, in string data)
        {
            Id = id;
            Event = @event;
            Data = data;
        }
    }
}