using System;

namespace Alphacloud.Common.Infrastructure.Caching
{
    /// <summary>
    /// Null Object for Cache
    /// </summary>
    internal class NullCache : CacheBase
    {
        public NullCache()
            : base(NullCacheHealthcheckMonitor.Instance, "<Null>")
        {
        }

        protected internal override CacheStatistics DoGetStatistics()
        {
            return new CacheStatistics(false);
        }

        protected internal override object DoGet(string key)
        {
            return null;
        }

        protected internal override void DoClear()
        {
        }

        protected internal override void DoRemove(string key)
        {
        }

        protected internal override void DoPut(string key, object value, TimeSpan ttl)
        {
        }


    }
}