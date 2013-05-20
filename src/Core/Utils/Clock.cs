#region copyright

// Copyright 2013 Alphacloud.Net
// 
//    Licensed under the Apache License, Version 2.0 (the "License");
//    you may not use this file except in compliance with the License.
//    You may obtain a copy of the License at
// 
//        http://www.apache.org/licenses/LICENSE-2.0
// 
//    Unless required by applicable law or agreed to in writing, software
//    distributed under the License is distributed on an "AS IS" BASIS,
//    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//    See the License for the specific language governing permissions and
//    limitations under the License.

#endregion

namespace Alphacloud.Common.Core.Utils
{
    using System;

    using JetBrains.Annotations;

    /// <summary>
    ///   Current date time hook.
    ///   Used to hook into DateTime.Now and DateTime.UtcNow.
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
        ///   Returns current time. See <see cref="DateTime.Now" />.
        /// </summary>
        public static Func<DateTime> CurrentTime { get; set; }


        /// <summary>
        ///   Returns current time in UTC format. See <see cref="DateTime.UtcNow" />.
        /// </summary>
        public static Func<DateTime> CurrentTimeUtc { get; set; }

        /// <summary>
        ///   Returns current time with time zone info. See <see cref="DateTimeOffset.Now" />.
        /// </summary>
        public static Func<DateTimeOffset> CurrentTimeWithZone { get; set; }


        /// <summary>
        ///   Resets current date time functions to use DateTime.Now and DateTime.UtcNow.
        /// </summary>
        public static void Reset()
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
