namespace Estudos.SSE.Core.Options
{
    internal sealed class DistributedCacheClientSseStorageOptions
    {
        private const int MaxTimeCacheInMinutesDefault = 5;

        private int _maxTimeCacheInMinutes;

        public int MaxTimeCacheInMinutes
        {
            get => _maxTimeCacheInMinutes;
            set => _maxTimeCacheInMinutes = value == default ? MaxTimeCacheInMinutesDefault : value;
        }
    }
}