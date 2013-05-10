namespace Alphacloud.Common.Core.Utils
{
    #region using

    using System;

    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///     Current date time hook.
    ///     Used to hook into DateTime.Now and DateTime.UtcNow.
    /// </summary>
    [PublicAPI]
    public static class Clock
    {
        #region .ctor

        static Clock()
        {
            Reset();
        }

        #endregion

        /// <summary>
        ///     Returns current time. See <see cref="DateTime.Now" />.
        /// </summary>
        public static Func<DateTime> CurrentTime { get; internal set; }


        /// <summary>
        ///     Returns current time in UTC format. See <see cref="DateTime.UtcNow" />.
        /// </summary>
        public static Func<DateTime> CurrentTimeUtc { get; internal set; }

        /// <summary>
        ///     Returns current time with time zone info. See <see cref="DateTimeOffset.Now" />.
        /// </summary>
        public static Func<DateTimeOffset> CurrentTimeWithZone { get; internal set; }


        /// <summary>
        ///     Resets current date time functions to use DateTime.Now and DateTime.UtcNow.
        /// </summary>
        internal static void Reset()
        {
            CurrentTime = GetCurrentLocalTime;
            CurrentTimeUtc = GetCurrentUtcTime;
            CurrentTimeWithZone = GetCurrentTimeWithTimeZone;
        }


        static DateTime GetCurrentLocalTime()
        {
            return DateTime.Now;
        }


        static DateTime GetCurrentUtcTime()
        {
            return DateTime.UtcNow;
        }


        static DateTimeOffset GetCurrentTimeWithTimeZone()
        {
            return DateTimeOffset.Now;
        }
    }
}
