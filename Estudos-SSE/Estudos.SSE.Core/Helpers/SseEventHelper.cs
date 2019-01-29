using System.Globalization;
using System.Text;
using Estudos.SSE.Core.Events;

// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Core.Helpers
{
    internal static class SseEventHelper
    {
        private const byte Cr = 13;
        private const byte Lf = 10;
        private const int CrlfLenght = 2;

        private static readonly byte[] SseRetryField = Encoding.UTF8.GetBytes(SseConstants.SseRetryField);
        private static readonly byte[] SseCommentField = Encoding.UTF8.GetBytes(SseConstants.SseCommentField);
        private static readonly byte[] SseIdField = Encoding.UTF8.GetBytes(SseConstants.SseIdField);
        private static readonly byte[] SseEventField = Encoding.UTF8.GetBytes(SseConstants.SseEventField);
        private static readonly byte[] SseDataField = Encoding.UTF8.GetBytes(SseConstants.SseDataField);

        public static SseEventBytes GetReconnectIntervalBytes(uint reconnectInterval)
        {
            var reconnectIntervalStringField = reconnectInterval.ToString(CultureInfo.InvariantCulture);

            var bytes = new byte[GetFieldMaxBytesCount(SseRetryField, reconnectIntervalStringField) + CrlfLenght];
            var bytesCount = GetFieldBytes(SseRetryField, reconnectIntervalStringField, bytes, 0);

            bytes[bytesCount++] = Cr;
            bytes[bytesCount++] = Lf;

            return new SseEventBytes(bytes, bytesCount);
        }

        public static SseEventBytes GetCommentBytes(string comment)
        {
            var bytes = new byte[GetFieldMaxBytesCount(SseCommentField, comment) + CrlfLenght];
            var bytesCount = GetFieldBytes(SseCommentField, comment, bytes, 0);

            bytes[bytesCount++] = Cr;
            bytes[bytesCount++] = Lf;

            return new SseEventBytes(bytes, bytesCount);
        }

        public static SseEventBytes GetEventBytes(string text)
        {
            var bytes = new byte[GetFieldMaxBytesCount(SseDataField, text) + CrlfLenght];
            var bytesCount = GetFieldBytes(SseDataField, text, bytes, 0);

            bytes[bytesCount++] = Cr;
            bytes[bytesCount++] = Lf;

            return new SseEventBytes(bytes, bytesCount);
        }

        public static SseEventBytes GetEventBytes(SseEvent serverSent)
        {
            var bytesCount = 0;
            var bytes = new byte[GetEventMaxBytesCount(serverSent)];

            if (!string.IsNullOrWhiteSpace(serverSent.Id))
            {
                bytesCount = GetFieldBytes(SseIdField, serverSent.Id, bytes, bytesCount);
            }

            bytesCount = GetFieldBytes(SseEventField, serverSent.Event, bytes, bytesCount);
            bytesCount = GetFieldBytes(SseDataField, serverSent.Data, bytes, bytesCount);

            bytes[bytesCount++] = Cr;
            bytes[bytesCount++] = Lf;

            return new SseEventBytes(bytes, bytesCount);
        }

        private static int GetEventMaxBytesCount(SseEvent serverSent)
        {
            var bytesCount = CrlfLenght;

            if (!string.IsNullOrWhiteSpace(serverSent.Id))
            {
                bytesCount += GetFieldMaxBytesCount(SseIdField, serverSent.Id);
            }

            bytesCount += GetFieldMaxBytesCount(SseEventField, serverSent.Event);
            bytesCount += GetFieldMaxBytesCount(SseDataField, serverSent.Data);

            return bytesCount;
        }

        private static int GetFieldBytes(byte[] field, string data, byte[] bytes, int bytesCount)
        {
            for (var fieldIndex = 0; fieldIndex < field.Length; fieldIndex++)
            {
                bytes[bytesCount++] = field[fieldIndex];
            }

            bytesCount += Encoding.UTF8.GetBytes(data, 0, data.Length, bytes, bytesCount);

            bytes[bytesCount++] = Cr;
            bytes[bytesCount++] = Lf;

            return bytesCount;
        }

        private static int GetFieldMaxBytesCount(byte[] field, string data)
        {
            return field.Length + Encoding.UTF8.GetMaxByteCount(data.Length) + CrlfLenght;
        }
    }
}