// ReSharper disable InconsistentNaming

namespace Estudos.SSE.Core
{
    internal static class SseConstants
    {
        public const string DisabledBufferingHeader = "rx-disabled-bufering";
        
        public const string DisabledBuffered = "true";

        public const string HandledContentResultHeader = "rx-handled-content-result";
        
        public const string HandledContentResult = "true";
        
        public const string ContentEncodingHeader = "Content-Encoding";

        public const string IdentityContentEncoding = "identity";
    
        public const string CacheControl = "Cache-Control";
    
        public const string Connection = "Connection";

        public const string SseContentType = "text/event-stream";
    
        public const string SseConnection = "keep-alive";

        public const string SseCacheControl = "no-cache";

        public const string LastEventIdHttpHeader = "Last-Event-ID";

        public const string SseRetryField = "retry: ";

        public const string SseCommentField = ": ";

        public const string SseIdField = "id: ";

        public const string SseEventField = "event: ";

        public const string SseDataField = "data: ";
    }
}