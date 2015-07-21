#region copyright

// Copyright 2013-2014 Alphacloud.Net
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

namespace Alphacloud.Common.Web.Mvc.Caching
{
    using System;
    using System.Globalization;
    using System.Web;
    using Core.Data;
    using JetBrains.Annotations;


    /// <summary>
    /// 
    /// </summary>
    [PublicAPI]
    public static class HttpRequestExtensions
    {
        /// <summary>
        ///   Determines whether resource was modified since last request.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="date">Last modified reaource date (UTC).</param>
        /// <returns>
        ///   <c>true</c> if resource was modified; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">request</exception>
        public static bool WasModifiedSince([NotNull] this HttpRequestBase request, DateTime date)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }

            DateTime lastModified;
            if (!DateTime.TryParse(request.Headers["If-Modified-Since"],
                CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal,
                out lastModified))
            {
                return true;
            }
            return lastModified < date.ToUniversalTime().StripMilliseconds();
        }


        /// <summary>
        ///   Checks if ETag matches current.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <param name="etag">The ETag.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        ///   request
        ///   or
        ///   etag
        /// </exception>
        public static bool EtagHasBeenChanged([NotNull] this HttpRequestBase request, [NotNull] string etag)
        {
            if (request == null)
            {
                throw new ArgumentNullException("request");
            }
            if (etag == null)
            {
                throw new ArgumentNullException("etag");
            }
            var ifNoneMatch = request.Headers["If-None-Match"];
            return ifNoneMatch != etag;
        }
    }
}