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

namespace Alphacloud.Common.Core.Data
{
    using System;

    /// <summary>
    ///   Week days mask.
    /// </summary>
    [Flags]
    public enum Week : byte
    {
        /// <summary>
        ///   None of week days selected.
        /// </summary>
        None = 0,

        /// <summary>
        ///   Monday
        /// </summary>
        Monday = 0x01,

        /// <summary>
        ///   Tuesday
        /// </summary>
        Tuesday = 0x02,

        /// <summary>
        ///   Wednesday
        /// </summary>
        Wednesday = 0x04,

        /// <summary>
        ///   Thursday
        /// </summary>
        Thursday = 0x08,

        /// <summary>
        ///   Friday
        /// </summary>
        Friday = 0x10,

        /// <summary>
        ///   Saturday
        /// </summary>
        Saturday = 0x20,

        /// <summary>
        ///   Sunday
        /// </summary>
        Sunday = 0x40,

        /// <summary>
        ///   Represend whole week.
        /// </summary>
        WholeWeek = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday
    }


    /// <summary>
    ///   Week daks mask.
    /// </summary>
    [Serializable]
    public class WeekDays : IEquatable<WeekDays>
    {
        #region .ctor

        /// <summary>
        ///   Initializes a new instance of the <see cref="WeekDays" /> class.
        /// </summary>
        /// <param name="week">The week.</param>
        public WeekDays(Week week)
        {
            SetWeek(week);
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="WeekDays" /> class.
        /// </summary>
        public WeekDays()
        {
            SetWeek(Week.None);
        }

        #endregion

        /// <summary>
        ///   Is Monday selected.
        /// </summary>
        public bool Monday { get; set; }

        /// <summary>
        ///   Is Tuesday selected.
        /// </summary>
        public bool Tuesday { get; set; }

        /// <summary>
        ///   Is Wednesday selected.
        /// </summary>
        public bool Wednesday { get; set; }

        /// <summary>
        ///   Is Thursday selected.
        /// </summary>
        public bool Thursday { get; set; }

        /// <summary>
        ///   Is Friday selected.
        /// </summary>
        public bool Friday { get; set; }

        /// <summary>
        ///   Is Saturday selected.
        /// </summary>
        public bool Saturday { get; set; }

        /// <summary>
        ///   Is Sunday selected.
        /// </summary>
        public bool Sunday { get; set; }

        #region IEquatable<WeekDays> Members

        /// <summary>
        ///   Determines whether the specified <see cref="WeekDays" /> is equal to this instance.
        /// </summary>
        /// <param name="other">
        ///   The <see cref="WeekDays" /> to compare with this instance.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="WeekDays" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public bool Equals(WeekDays other)
        {
            if (ReferenceEquals(null, other))
                return false;
            if (ReferenceEquals(this, other))
                return true;
            return other.Monday == Monday && other.Tuesday == Tuesday && other.Wednesday == Wednesday
                && other.Thursday == Thursday
                && other.Friday == Friday && other.Saturday == Saturday && other.Sunday == Sunday;
        }

        #endregion

        /// <summary>
        ///   Set mask from <paramref name="week" />.
        /// </summary>
        /// <param name="week">Week days mask.</param>
        void SetWeek(Week week)
        {
            Monday = (week & Week.Monday) == Week.Monday;
            Tuesday = (week & Week.Tuesday) == Week.Tuesday;
            Wednesday = (week & Week.Wednesday) == Week.Wednesday;
            Thursday = (week & Week.Thursday) == Week.Thursday;
            Friday = (week & Week.Friday) == Week.Friday;
            Saturday = (week & Week.Saturday) == Week.Saturday;
            Sunday = (week & Week.Sunday) == Week.Sunday;
        }


        /// <summary>
        ///   Determines at least one day is selected.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if selection is empty; otherwise, <c>false</c>.
        /// </returns>
        public bool IsEmpty()
        {
            return !(Monday || Tuesday || Wednesday || Thursday || Friday || Saturday || Sunday);
        }


        /// <summary>
        ///   Convert to <see cref="Week" /> flags.
        /// </summary>
        /// <returns></returns>
        public Week Pack()
        {
            var week = Week.None;
            if (Monday)
                week |= Week.Monday;
            if (Tuesday)
                week |= Week.Tuesday;
            if (Wednesday)
                week |= Week.Wednesday;
            if (Thursday)
                week |= Week.Thursday;
            if (Friday)
                week |= Week.Friday;
            if (Saturday)
                week |= Week.Saturday;
            if (Sunday)
                week |= Week.Sunday;
            return week;
        }


        /// <summary>
        ///   Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">
        ///   The <see cref="System.Object" /> to compare with this instance.
        /// </param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
                return false;
            if (ReferenceEquals(this, obj))
                return true;
            if (obj.GetType() != typeof (WeekDays))
                return false;
            return Equals((WeekDays) obj);
        }


        /// <summary>
        ///   Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        ///   A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
        /// </returns>
        public override int GetHashCode()
        {
            return (int) Pack();
        }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents week days.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents week days.
        /// </returns>
        public override string ToString()
        {
            return Pack().ToString();
        }
    }
}
