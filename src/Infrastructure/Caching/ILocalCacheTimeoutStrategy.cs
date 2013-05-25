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
        ///     Gets timeout for local cache item.
        /// </summary>
        /// <param name="ttl">Shared cache timeout.</param>
        /// <returns>Local cache timeout.</returns>
        TimeSpan GetLocalTimeout(TimeSpan ttl);
    }
}