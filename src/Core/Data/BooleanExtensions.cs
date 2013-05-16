namespace Alphacloud.Common.Core.Data
{
    #region using

    using System;
    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///   <c>bool</c> data type extensions.
    /// </summary>
    [PublicAPI]
    public static class BooleanExtensions
    {
        /// <summary>
        ///   Convery boolean value to yes/no string.
        /// </summary>
        /// <returns>
        ///   Yes for <c>true</c>, no for <c>false</c>.
        /// </returns>
        public static string YesNo(this bool value)
        {
            return value.YesNo("yes", "no");
        }

        /// <summary>
        ///   Return Yes/No text
        /// </summary>
        /// <param name="value">
        ///   if set to <c>true</c> [value].
        /// </param>
        /// <param name="yes">The yes.</param>
        /// <param name="no">The no.</param>
        /// <returns></returns>
        public static string YesNo(this bool value, string yes, string no)
        {
            return value ? yes : no;
        }

        /// <summary>
        ///   Return yes/no string with optional evaluation of No-string.
        ///   Use for case when No-string appears rarely and requires additional effort to evaluate.
        /// </summary>
        /// <param name="value">boolean.</param>
        /// <param name="yes">Yes-string.</param>
        /// <param name="no">No-string evaluation callback.</param>
        /// <returns></returns>
        public static string YesNo(this bool value, string yes, [NotNull] Func<string> no)
        {
            if (no == null)
                throw new ArgumentNullException("no");
            return value ? yes : no();
        }

        /// <summary>
        ///   Convert to 1/0.
        /// </summary>
        public static int AsInt(this bool value)
        {
            return value ? 1 : 0;
        }
    }
}