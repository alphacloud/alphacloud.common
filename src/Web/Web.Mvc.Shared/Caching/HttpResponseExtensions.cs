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
    using System.Web;
    using Core.Data;
    using JetBrains.Annotations;


    /// <summary>
    ///   Http response extention methods.
    /// </summary>
    [PublicAPI]
    public static class HttpResponseExtensions
    {
        /// <summary>
        ///   Sets the LastModified field.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="lastModified">The last modified.</param>
        /// <returns>Rounded LastModified as UTC time.</returns>
        /// <exception cref="System.ArgumentNullException">response</exception>
        public static DateTime SetLastModified([NotNull] this HttpResponseBase response, DateTime lastModified)
        {
            if (response == null)
            {
                throw new ArgumentNullException("response");
            }

            response.Cache.SetLastModified(lastModified);
            return lastModified.StripMilliseconds();
        }
    }
}