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
    using System.Globalization;

    /// <summary>
    ///   Number formatting extensions
    /// </summary>
    public static class NumberFormattingExtensions
    {
        /// <summary>
        ///   Format value using neutral culture.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> </returns>
        public static string AsStr(this double value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }


        /// <summary>
        ///   Format value using neutral culture.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> string </returns>
        public static string AsStr(this int value)
        {
            return value.ToString(CultureInfo.InvariantCulture);
        }
    }
}
