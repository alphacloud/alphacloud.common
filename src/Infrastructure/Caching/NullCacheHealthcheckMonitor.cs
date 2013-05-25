namespace Alphacloud.Common.Infrastructure.Caching
{
    /// <summary>
    ///     Stub for healthceck monitor, reporting cache is always available.
    /// </summary>
    public class NullCacheHealthcheckMonitor : ICacheHealthcheckMonitor
    {
        public static readonly ICacheHealthcheckMonitor Instance = new NullCacheHealthcheckMonitor();

        NullCacheHealthcheckMonitor()
        {
        }

        public void Start()
        {
        }

        public bool IsCacheAvailable
        {
            get { return true; }
        }
    }
}
