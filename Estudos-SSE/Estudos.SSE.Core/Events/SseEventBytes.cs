// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Core.Events
{
    public readonly struct SseEventBytes
    {
        public byte[] Bytes { get; }

        public int BytesCount { get; }

        public SseEventBytes(in byte[] bytes, in int bytesCount)
        {
            Bytes = bytes;
            BytesCount = bytesCount;
        }
    }
}