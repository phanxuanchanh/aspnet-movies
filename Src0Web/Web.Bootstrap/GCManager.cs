using System;

namespace Web
{
    public class GCManager
    {
        private static int _requestCount = 0;
        private static readonly object _lock = new object();

        public const int RequestThreshold = 300;

        public static void CollectIfThresholdReached()
        {
            lock (_lock)
            {
                _requestCount++;
                if (_requestCount >= RequestThreshold)
                {
                    GC.Collect();
                    _requestCount = 0;
                }
            }
        }
    }
}