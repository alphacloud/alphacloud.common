namespace Alphacloud.Common.Infrastructure.Caching
{
    using System;
    using Core.Data;
    using JetBrains.Annotations;

    /// <summary>
    ///     Provides fixed timeout for local cache.
    /// </summary>
    [PublicAPI]
    [UsedImplicitly]
    public class FixedTimeoutStrategy : ILocalCacheTimeoutStrategy
    {
        private readonly TimeSpan _timeout;

        /// <summary>
        ///     Initializes a new instance of the <see cref="FixedTimeoutStrategy" /> class.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public FixedTimeoutStrategy(TimeSpan timeout)
        {
            _timeout = timeout;
        }

        /// <summary>
        /// Calculate timeout for local cache item.
        /// </summary>
        /// <param name="ttl">Base cache item timeout.</param>
        /// <returns>
        /// Fixed timeout.
        /// </returns>
        public TimeSpan GetLocalTimeout(TimeSpan ttl)
        {
            if (ttl == TimeSpan.Zero)
                return _timeout;
            return (ttl > _timeout ? _timeout : ttl);
        }


        /// <summary>
        /// Text description of timeout calculation strategy.
        /// </summary>
        /// <returns></returns>
        public string Describe()
        {
            return "Fixed ({0} seconds)".ApplyArgs(_timeout.TotalSeconds);
        }
    }
}