namespace Alphacloud.Common.Core.Data
{
    using System;

    using Alphacloud.Common.Core.Strings;
    using Alphacloud.Common.Core.Utils;

    using JetBrains.Annotations;

    /// <summary>
    /// DateTime extensions
    /// </summary>
    [PublicAPI]
    public static class DateTimeExtensions
    {
        /// <summary>
        ///   Rounds minutes to the specified boundary.
        /// </summary>
        /// <param name="dt">DateTime.</param>
        /// <param name="minutes">Round value.</param>
        /// <returns>DateTime rounded to specified value.</returns>
        public static DateTime RoundMinutes(this DateTime dt, int minutes)
        {
            var min = dt.Minute % minutes;
            var seconds = -1.0 * dt.Second;
            return (min) < (minutes + 1) / 2
                ? dt.AddMinutes(-min).AddSeconds(seconds)
                : dt.AddMinutes(minutes - min).AddSeconds(seconds);
        }

        /// <summary>
        ///   Convert to Date and Time string for current locale.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateTimeStr(this DateTime dateTime)
        {
            return String.Concat(dateTime.ToShortDateString(), " ", dateTime.ToShortTimeString());
        }


        /// <summary>
        ///   Convert date and time to long datetime string.
        /// </summary>
        /// <param name="dateTime">Date time</param>
        /// <returns>
        ///   Long datetime formatted string (corresponds to <c>dateTime.ToString("f")</c>).
        /// </returns>
        public static string ToLongDateTimeStr(this DateTime dateTime)
        {
            return dateTime.ToString("f");
        }

        /// <summary>
        ///   Convert to locate time zone and display as date time string.
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <returns>date time string</returns>
        public static string ToLocalDateTimeStr(this DateTime dateTime)
        {
            var localTime = dateTime.ToLocalTime();
            return string.Concat(localTime.ToShortTimeString(), " ", localTime.ToShortDateString());
        }

        /// <summary>
        ///   Convert to relative human-readable date.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string AsRelativeDateTime(this DateTime dateTime)
        {
            var current = Clock.CurrentTime();
            var diff = dateTime.Subtract(current);
            if (diff.TotalHours.InRange(0, 1))
                return Strings.RelativeDate_ThisHours;
            if (diff.TotalHours.InRange(-1, 0))
                return Strings.RelativeDate_HourAgo;
            if (diff.TotalDays.InRange(-1, 1))
                return Strings.RelativeDate_Today;
            return dateTime.ToShortDateString();
        }

        /// <summary>
        /// Format datatime as relative to current.
        /// </summary>
        /// <param name="dateTime">DateTime</param>
        /// <param name="emptyDate">Empty data value.</param>
        /// <returns>Return relative date or <paramref name="emptyDate"/> if date was not provided.</returns>
        public static string AsHumanReadable(this DateTime? dateTime, string emptyDate = "")
        {
            return dateTime.HasValue ? dateTime.Value.AsRelativeDateTime() : emptyDate;
        }

        /// <summary>
        ///     Strips the milliseconds from datetime.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns></returns>
        public static DateTime StripMilliseconds(this DateTime dateTime)
        {
            var result = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, dateTime.Hour,
                dateTime.Minute, dateTime.Second, dateTime.Kind);
            return result;
        }

    }
}