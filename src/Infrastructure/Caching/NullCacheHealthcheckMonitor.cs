namespace Alphacloud.Common.Infrastructure.Caching
{
    /// <summary>
    ///     Stub for healthceck monitor, reporting cache is always available.
    /// </summary>
    public sealed class NullCacheHealthcheckMonitor : ICacheHealthcheckMonitor
    {
        /// <summary>
        /// Null Object implementation for health-check monitor.
        /// </summary>
        public static readonly ICacheHealthcheckMonitor Instance = new NullCacheHealthcheckMonitor();

        NullCacheHealthcheckMonitor()
        {
        }

        /// <summary>
        /// Start monitoring.
        /// </summary>
        public void Start()
        {
        }

        /// <summary>
        /// Always reports cache is avaiable in this stub.
        /// </summary>
        public bool IsCacheAvailable
        {
            get { return true; }
        }
    }
}
