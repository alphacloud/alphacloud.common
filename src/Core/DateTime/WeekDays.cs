namespace Alphacloud.Common.Core.DateTime
{
    #region using

    using System;

    #endregion

    [Flags]
    public enum Week : byte
    {
        None = 0,
        Monday = 0x01,
        Tuesday = 0x02,
        Wednesday = 0x04,
        Thursday = 0x08,
        Friday = 0x10,
        Saturday = 0x20,
        Sunday = 0x40,
        WholeWeek = Monday | Tuesday | Wednesday | Thursday | Friday | Saturday | Sunday
    }


    [Serializable]
    public class WeekDays : IEquatable<WeekDays>
    {
        #region .ctor

        public WeekDays(Week week)
        {
            SetWeek(week);
        }


        public WeekDays()
        {
            SetWeek(Week.None);
        }

        #endregion

        public bool Monday { get; set; }
        public bool Tuesday { get; set; }
        public bool Wednesday { get; set; }
        public bool Thursday { get; set; }
        public bool Friday { get; set; }
        public bool Saturday { get; set; }
        public bool Sunday { get; set; }

        #region IEquatable<WeekDays> Members

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

        private void SetWeek(Week week)
        {
            Monday = (week & Week.Monday) == Week.Monday;
            Tuesday = (week & Week.Tuesday) == Week.Tuesday;
            Wednesday = (week & Week.Wednesday) == Week.Wednesday;
            Thursday = (week & Week.Thursday) == Week.Thursday;
            Friday = (week & Week.Friday) == Week.Friday;
            Saturday = (week & Week.Saturday) == Week.Saturday;
            Sunday = (week & Week.Sunday) == Week.Sunday;
        }


        public bool IsEmpty()
        {
            return !(Monday || Tuesday || Wednesday || Thursday || Friday || Saturday || Sunday);
        }


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


        public override int GetHashCode()
        {
            unchecked
            {
                int result = Monday.GetHashCode();
                result = (result*397) ^ Tuesday.GetHashCode();
                result = (result*397) ^ Wednesday.GetHashCode();
                result = (result*397) ^ Thursday.GetHashCode();
                result = (result*397) ^ Friday.GetHashCode();
                result = (result*397) ^ Saturday.GetHashCode();
                result = (result*397) ^ Sunday.GetHashCode();
                return result;
            }
        }
    }
}