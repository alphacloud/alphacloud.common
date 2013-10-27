namespace Alphacloud.Common.Core.Data.Hash
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    /// <summary>
    /// generic fast hash algorithm.
    /// </summary>
    public interface IUintHash
    {
        /// <summary>
        /// Computes the hash for specified array.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="start">The start position.</param>
        /// <param name="size">The size.</param>
        /// <returns>32bit hash</returns>
        uint Compute([NotNull] byte[] array, int start, int size);

        /// <summary>
        /// Computes the hash for specified array.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>32bit hash</returns>
        uint Compute([NotNull] int[] data);
        /// <summary>
        /// Computes the hash for specified data.
        /// Use this overload to avoid calling.ToArray()
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>32bit hash</returns>
        uint Compute([NotNull] IList<int> data);
    }
}