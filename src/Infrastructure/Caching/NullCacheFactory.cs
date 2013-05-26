using Common.Logging;

namespace Alphacloud.Common.Infrastructure.Caching
{
    using JetBrains.Annotations;

    /// <summary>
    /// Null cache factory.
    /// </summary>
    [PublicAPI]
    public sealed class NullCacheFactory: ICacheFactory

    {
        private static readonly ILog s_log = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// Returns NullCache implementation.
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public ICache GetCache(string instance = null)
        {
            return CacheBase.Null;
        }

        /// <summary>
        /// Initialization.
        /// </summary>
        public void Initialize()
        {
            s_log.Info("Initialize cache");
        }
    }
}