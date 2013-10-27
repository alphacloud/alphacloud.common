namespace Alphacloud.Common.Infrastructure.Caching
{
    /// <summary>
    ///     Cache healthcehck monitor.
    /// </summary>
    public interface ICacheHealthcheckMonitor
    {
        /// <summary>
        ///     Gets a value indicating whether cache is available.
        /// </summary>
        /// <value>
        ///     <c>true</c> if cache is available; otherwise, <c>false</c>.
        /// </value>
        bool IsCacheAvailable { get; }

        /// <summary>
        ///     Start monitoring.
        /// </summary>
        void Start();
    }
}
