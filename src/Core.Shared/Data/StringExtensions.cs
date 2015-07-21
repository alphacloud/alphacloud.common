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
    #region using

    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;
    using JetBrains.Annotations;

    #endregion

    /// <summary>
    ///   String extension methods.
    /// </summary>
    [PublicAPI]
    public static class StringExtensions
    {
        /// <summary>
        ///   Format string using Invariant Culture.
        /// </summary>
        /// <param name="fmt">Format specifier.</param>
        /// <param name="arg0">The arg0.</param>
        /// <returns>Formatted string.</returns>
        [StringFormatMethod("fmt")]
        public static string ApplyArgs(this string fmt, object arg0)
        {
            return string.Format(CultureInfo.InvariantCulture, fmt, arg0);
        }


        /// <summary>
        ///   Format string using Invariant Culture.
        /// </summary>
        /// <param name="fmt">Format specifier.</param>
        /// <param name="arg0">The arg0.</param>
        /// <param name="arg1"></param>
        /// <returns>Formatted string.</returns>
        [StringFormatMethod("fmt")]
        public static string ApplyArgs(this string fmt, object arg0, object arg1)
        {
            return string.Format(CultureInfo.InvariantCulture, fmt, arg0, arg1);
        }


        /// <summary>
        ///   Format string using Invariant Culture.
        /// </summary>
        /// <param name="fmt">Format specifier.</param>
        /// <param name="arg0">Arg 0.</param>
        /// <param name="arg1">Arg 1.</param>
        /// <param name="arg2">Arg 2.</param>
        /// <returns>Formatted string.</returns>
        [StringFormatMethod("fmt")]
        public static string ApplyArgs(this string fmt, object arg0, object arg1, object arg2)
        {
            return string.Format(CultureInfo.InvariantCulture, fmt, arg0, arg1, arg2);
        }


        /// <summary>
        ///   Format string using Invariant Culture.
        /// </summary>
        /// <param name="fmt">Format specifier.</param>
        /// <param name="args">Args.</param>
        /// <returns>Formatted string.</returns>
        [StringFormatMethod("fmt")]
        public static string ApplyArgs(this string fmt, params object[] args)
        {
            return string.Format(CultureInfo.InvariantCulture, fmt, args);
        }


        /// <summary>
        ///   Format string using Current Culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">The arg0.</param>
        /// <returns>Formatted string.</returns>
        [StringFormatMethod("fmt")]
        public static string Fmt(this string format, object arg0)
        {
            return string.Format(CultureInfo.CurrentCulture, format, arg0);
        }


        /// <summary>
        ///   Format string using Current Culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Arg 0</param>
        /// <param name="arg1">Arg 1</param>
        /// <returns>Formatted string.</returns>
        [StringFormatMethod("fmt")]
        public static string Fmt(this string format, object arg0, object arg1)
        {
            return string.Format(CultureInfo.CurrentCulture, format, arg0, arg1);
        }


        /// <summary>
        ///   Format string using Current Culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="arg0">Arg 0</param>
        /// <param name="arg1">Arg 1</param>
        /// <param name="arg2"></param>
        /// <returns>Formatted string.</returns>
        [StringFormatMethod("fmt")]
        public static string Fmt(this string format, object arg0, object arg1, object arg2)
        {
            return string.Format(CultureInfo.CurrentCulture, format, arg0, arg1, arg2);
        }


        /// <summary>
        ///   Format string using Current Culture.
        /// </summary>
        /// <param name="format">Format specifier.</param>
        /// <param name="args">Arguments.</param>
        /// <returns>Formatted string.</returns>
        [StringFormatMethod("fmt")]
        public static string Fmt(this string format, params object[] args)
        {
            return string.Format(CultureInfo.CurrentCulture, format, args);
        }


        /// <summary>
        ///   Joins strings using separator.
        /// </summary>
        /// <param name="strings">Strings.</param>
        /// <param name="separator">String separator.</param>
        /// <returns>StringBuilder</returns>
        public static StringBuilder JoinStrings(this IEnumerable<string> strings, string separator)
        {
            var sb = new StringBuilder();
            foreach (var str in strings)
            {
                if (sb.Length > 0)
                    sb.Append(separator);
                sb.Append(str);
            }
            return sb;
        }


        /// <summary>
        ///   Truncate string to specified length, adding <paramref name="terminator" /> if string was truncated.
        /// </summary>
        /// <param name="str">String to truncate.</param>
        /// <param name="maxLength">Maximum length.</param>
        /// <param name="terminator">The terminator to add if string was truncated.</param>
        /// <returns>Truncated string</returns>
        /// <exception cref="System.ArgumentException">@Terminator cannot be longer than maxLength.;terminator</exception>
        public static string Truncate(this string str, int maxLength, string terminator = "...")
        {
            if (string.IsNullOrEmpty(str))
                return string.Empty;
            terminator = terminator ?? string.Empty;
            if (terminator.Length >= maxLength)
                throw new ArgumentException(@"Terminator cannot be longer than maxLength.", "terminator");
            return str.Length <= maxLength
                ? str
                : str.Substring(0, maxLength - terminator.Length) + terminator;
        }


        /// <summary>
        ///   Substitutes null or empty string.
        /// </summary>
        /// <param name="str">String to check.</param>
        /// <param name="substitute">The substitute value.</param>
        /// <returns></returns>
        public static string IfEmpty(this string str, string substitute)
        {
            return string.IsNullOrEmpty(str)
                ? substitute
                : str;
        }


        /// <summary>
        ///   Trim string if it is not null or whitespace.
        ///   Behaves like <see cref="string.Trim()" /> except no error thrown in str == <c>null</c>.
        /// </summary>
        /// <param name="str">Input.</param>
        /// <returns></returns>
        public static string TryTrim(this string str)
        {
            return string.IsNullOrWhiteSpace(str)
                ? string.Empty
                : str.Trim();
        }


        /// <summary>
        ///   Determines whether string contains substring ignoring case.
        /// </summary>
        /// <param name="str">The string.</param>
        /// <param name="substring">The substring.</param>
        /// <returns>
        ///   <c>true</c> if string conrains substring; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        ///   <paramref name="str" />  or
        ///   <paramref name="substring" /> is <c>null</c>.
        /// </exception>
        public static bool ContainsIgnoreCase([NotNull] this string str, [NotNull] string substring)
        {
            if (str == null) throw new ArgumentNullException("str");
            if (substring == null) throw new ArgumentNullException("substring");

            return str.IndexOf(str, StringComparison.OrdinalIgnoreCase) >= 0;
        }


        /// <summary>
        ///   Compare strings usingcase-insensitive comparison.
        /// </summary>
        /// <param name="strA">First string.</param>
        /// <param name="strB">Second string.</param>
        /// <returns><c>true</c> if strings are equal, <c>false</c> otherwise.</returns>
        public static bool EqualsIgnoreCase(this string strA, string strB)
        {
            return string.CompareOrdinal(strA, strB) == 0;
        }
    }
}