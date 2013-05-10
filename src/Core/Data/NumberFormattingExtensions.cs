namespace KudaNado.Common.Data
{
    #region using

    using System.Globalization;

    #endregion

    public static class NumberFormattingExtensions
    {
        /// <summary>
        ///     Format value using neutral culture.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> </returns>
        public static string AsStr(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }


        /// <summary>
        ///     Format value using neutral culture.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> string </returns>
        public static string AsStr(this int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
