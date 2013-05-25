namespace Alphacloud.Common.Infrastructure.Caching
{
    using System;

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

        public TimeSpan GetLocalTimeout(TimeSpan ttl)
        {
            return _timeout;
        }
    }
}