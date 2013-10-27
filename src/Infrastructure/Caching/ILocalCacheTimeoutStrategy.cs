namespace Alphacloud.Common.Infrastructure.Caching
{
    using System;
    
    using JetBrains.Annotations;

    /// <summary>
    ///     Local cache timeout generation strategy.
    /// </summary>
    [PublicAPI]
    public interface ILocalCacheTimeoutStrategy
    {
        /// <summary>
        ///     Calculate timeout for local cache item.
        /// </summary>
        /// <param name="ttl">Base cache item timeout.</param>
        /// <returns>Local cache item timeout.</returns>
        TimeSpan GetLocalTimeout(TimeSpan ttl);

        /// <summary>
        /// Text description of timeout calculation strategy.
        /// </summary>
        /// <returns></returns>
        string Describe();
    }
}