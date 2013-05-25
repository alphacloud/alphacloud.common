using System;
using System.Diagnostics;

namespace Alphacloud.Common.Infrastructure.Caching
{
    /// <summary>
    /// Cache item with last updated date attached.
    /// </summary>
    /// <typeparam name="T">Data type</typeparam>
    [Serializable]
    [DebuggerDisplay("{Item} modified at {LastModifiedUtc}")]
    public class Cached<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Cached{T}"/> class.
        /// </summary>
        public Cached()
        {
            LastModifiedUtc = DateTime.UtcNow;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="Cached{T}"/> class.
        /// </summary>
        /// <param name="updateTimeUtc">Last modification time.</param>
        public Cached(DateTime updateTimeUtc)
        {
            LastModifiedUtc = updateTimeUtc;
        }


        /// <summary>
        /// Last data modification time.
        /// </summary>
        public DateTime LastModifiedUtc { get; protected set; }

        /// <summary>
        /// Gets or sets data item.
        /// </summary>
        /// <value>
        /// Data item.
        /// </value>
        public T Item { get; set; }
    }
}