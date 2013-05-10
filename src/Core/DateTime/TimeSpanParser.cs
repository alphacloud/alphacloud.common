namespace Alphacloud.Common.Core.DateTime
{
    #region using

    using System;
    using System.Globalization;
    using System.Text.RegularExpressions;

    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///     TimeSpan generic parser
    /// </summary>
    [PublicAPI]
    public static class TimeSpanParser
    {
        const DateTimeStyles DefaultDateTimeStyles =
            DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal | DateTimeStyles.NoCurrentDateDefault;


        [PublicAPI]
        public static bool TryParseTime(string text, out TimeSpan time)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            var formats = new[] {
                "HH:mm", "HH.mm", "HHmm", "HH,mm", "HH",
                "H:mm", "H.mm", "H,mm",
                "hh:mmtt", "hh.mmtt", "hhmmtt", "hh,mmtt", "hhtt",
                "h:mmtt", "h.mmtt", "hmmtt", "h,mmtt", "htt"
            };

            text = Regex.Replace(text, "([^0-9]|^)([0-9])([0-9]{2})([^0-9]|$)", "$1$2:$3$4", RegexOptions.Compiled);
            text = Regex.Replace(text, "^[0-9]$", "0$0", RegexOptions.Compiled);

            foreach (var format in formats)
            {
                DateTime value;
                if (
                    !DateTime.TryParseExact(text, format, CultureInfo.InvariantCulture, DefaultDateTimeStyles, out value))
                    continue;
                time = value.TimeOfDay;
                return true;
            }
            time = TimeSpan.Zero;
            return false;
        }
    }
}
