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
    using System.Globalization;
    using System.Text.RegularExpressions;

    using JetBrains.Annotations;

    /// <summary>
    ///   TimeSpan generic parser.
    /// </summary>
    [PublicAPI]
    public static class TimeSpanParser
    {
        static readonly string[] s_formats = {
            "HH:mm", "HH.mm", "HHmm", "HH,mm", "HH",
            "H:mm", "H.mm", "H,mm",
            "hh:mmtt", "hh.mmtt", "hhmmtt", "hh,mmtt", "hhtt",
            "h:mmtt", "h.mmtt", "hmmtt", "h,mmtt", "htt"
        };

        const DateTimeStyles DefaultDateTimeStyles =
            DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeLocal | DateTimeStyles.NoCurrentDateDefault;


        /// <summary>
        /// Safe time parser.
        /// </summary>
        /// <param name="text">Text representation of time.</param>
        /// <param name="time">Parsed time value.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">text</exception>
        [PublicAPI]
        public static bool TryParseTime(string text, out TimeSpan time)
        {
            if (text == null)
                throw new ArgumentNullException("text");

            text = Regex.Replace(text, "([^0-9]|^)([0-9])([0-9]{2})([^0-9]|$)", "$1$2:$3$4", RegexOptions.Compiled);
            text = Regex.Replace(text, "^[0-9]$", "0$0", RegexOptions.Compiled);

            foreach (var format in s_formats)
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
