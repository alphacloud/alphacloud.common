namespace Alphacloud.Common.Core.Data
{
    public static class NumberExtensions
    {
        /// <summary>
        ///   Determine whether number is in specified range (inclusive).
        /// </summary>
        /// <param name="number">The number.</param>
        /// <param name="min">The min value.</param>
        /// <param name="max">The max value.</param>
        /// <returns></returns>
        public static bool InRange(this double number, double min, double max)
        {
            return number >= min && number <= max;
        }
    }
}