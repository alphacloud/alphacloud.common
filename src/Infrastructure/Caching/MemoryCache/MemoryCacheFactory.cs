namespace Alphacloud.Common.Infrastructure.Caching.MemoryCache
{
    using JetBrains.Annotations;

    /// <summary>
    ///     .NET MemoryCache factory
    /// </summary>
    [PublicAPI]
    public class MemoryCacheFactory : CacheFactoryBase
    {
        #region Overrides of CacheFactoryBase

        public override void Initialize()
        {
            Log.Info("Initialized");
        }


        protected override ICache CreateCache(string instance)
        {
            if (!IsEnabled)
            {
                return base.CreateCache(instance);
            }

            var memCache = new System.Runtime.Caching.MemoryCache(instance);
            var adapter = new MemoryCacheAdapter(memCache, NullCacheHealthcheckMonitor.Instance, instance);
            return adapter;
        }

        #endregion
    }
}