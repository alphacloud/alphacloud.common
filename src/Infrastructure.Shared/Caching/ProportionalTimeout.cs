namespace Alphacloud.Common.Infrastructure.Caching
{
    using System;
    using Core.Data;
    using JetBrains.Annotations;

    /// <summary>
    ///     Adaptive timeout.
    ///     Based on percentage of orininal with low and high limits.
    /// </summary>
    [PublicAPI]
    [UsedImplicitly]
    public class ProportionalTimeout : ILocalCacheTimeoutStrategy
    {
        private readonly TimeSpan _maxTtl;
        private readonly TimeSpan _minTtl;
        private readonly double _percentage;


        /// <summary>
        ///     Initializes a new instance of the <see cref="ProportionalTimeout" /> class using default values.
        ///     Local cache TTL is 10%, with range from 5 seconds to 5 minutes.
        /// </summary>
        public ProportionalTimeout() : this(TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(5), 10.0)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ProportionalTimeout" /> class.
        /// </summary>
        /// <param name="minTtl">The min TTL.</param>
        /// <param name="maxTtl">The max TTL.</param>
        /// <param name="percentage">The percentage.</param>
        public ProportionalTimeout(TimeSpan minTtl, TimeSpan maxTtl, double percentage)
        {
            if (percentage <= 0)
                throw new ArgumentOutOfRangeException("percentage", percentage, @"Percentage cannot be negative");

            _minTtl = minTtl;
            _maxTtl = maxTtl;
            _percentage = percentage / 100;
        }

        /// <summary>
        /// Gets timeout for local cache item.
        /// </summary>
        /// <param name="ttl">Shared cache timeout.</param>
        /// <returns>
        /// Local cache timeout.
        /// </returns>
        public TimeSpan GetLocalTimeout(TimeSpan ttl)
        {
            if (ttl == TimeSpan.Zero)
                return _minTtl;

            var localTtl = TimeSpan.FromMilliseconds(ttl.TotalMilliseconds * _percentage);

            if (localTtl < _minTtl)
                return _minTtl;
            if (localTtl > _maxTtl)
                return _maxTtl;
            return localTtl;
        }


        public string Describe()
        {
            return "Percentage ({0}% [{1}s .. {2}s])".ApplyArgs(_percentage, _minTtl.TotalSeconds, _maxTtl.TotalSeconds);
        }
    }
}