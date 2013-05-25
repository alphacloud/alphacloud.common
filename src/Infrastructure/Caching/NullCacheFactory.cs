using Common.Logging;

namespace Alphacloud.Common.Infrastructure.Caching
{
    /// <summary>
    /// Null cache factory.
    /// </summary>
    public class NullCacheFactory: ICacheFactory

    {
        private static readonly ILog s_log = LogManager.GetCurrentClassLogger();

        public ICache GetCache(string instance = null)
        {
            return CacheBase.Null;
        }

        public void Initialize()
        {
            s_log.Info("Initialize cache");
        }
    }
}