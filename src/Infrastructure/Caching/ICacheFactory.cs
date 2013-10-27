namespace Alphacloud.Common.Infrastructure.Caching
{
    /// <summary>
    ///     Factory to create cache.
    /// </summary>
    public interface ICacheFactory
    {
        /// <summary>
        ///     Get cache instance.
        /// </summary>
        /// <param name="instance">Instance name (optional)</param>
        /// <returns>Cache instance</returns>
        ICache GetCache(string instance = null);

        /// <summary>
        /// Initialize cache factory.
        /// </summary>
        void Initialize();
    }
}
