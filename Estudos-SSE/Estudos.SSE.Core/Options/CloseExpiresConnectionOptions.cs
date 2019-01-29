namespace Estudos.SSE.Core.Options
{
    internal sealed class CloseExpiresConnectionOptions
    {
        private const int CloseConnectionsInSecondsIntervalDefault = 60;

        private int _closeConnectionsInSecondsInterval;

        public int CloseConnectionsInSecondsInterval
        {
            get => _closeConnectionsInSecondsInterval;
            set => _closeConnectionsInSecondsInterval = value == default ? CloseConnectionsInSecondsIntervalDefault : value;
        }
    }
}