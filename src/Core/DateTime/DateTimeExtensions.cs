namespace Alphacloud.Common.Core.DateTime
{
    #region using

    using System;
    using Data;
    using JetBrains.Annotations;
    using Strings;

    #endregion

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
            var min = dt.Minute%minutes;
            return (min) < (minutes + 1)/2
                ? dt.AddMinutes(-min)
                : dt.AddMinutes(minutes - min);
        }

        /// <summary>
        ///   Convert to Date and Time string for current locale.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string ToDateTimeStr(this DateTime dateTime)
        {
            return dateTime.ToShortDateString() + " " + dateTime.ToShortTimeString();
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
            return "{0} {1}".ApplyArgs(localTime.ToShortTimeString(), localTime.ToShortDateString());
        }

        /// <summary>
        ///   Convert to relative human-readable date.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static string AsRelativeDateTime(this DateTime dateTime)
        {
            var current = DateTime.Now;
            var diff = dateTime.Subtract(current);
            if (diff.TotalHours.InRange(0, 1))
                return Strings.RelativeDate_ThisHours;
            if (diff.TotalHours.InRange(-1, 0))
                return Strings.RelativeDate_HourAgo;
            if (diff.TotalDays.InRange(-1, 1))
                return Strings.RelativeDate_Today;
            return dateTime.ToShortDateString();
        }

        public static string AsHumanReadable(this DateTime? dateTime, string emptyDate = "")
        {
            return dateTime.HasValue ? dateTime.Value.AsRelativeDateTime() : emptyDate;
        }
    }
}