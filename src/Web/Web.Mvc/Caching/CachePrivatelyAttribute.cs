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
    using System.Web.Mvc;


    /// <summary>
    ///   Tells browser to cache response privately.
    /// </summary>
    public class CachePrivatelyAttribute : ActionFilterAttribute
    {
        /// <summary>
        ///   Maximum age of cached data in seconds.
        /// </summary>
        /// <value>
        ///   The maximum age in seconds that a representation will be considered as fresh.
        /// </value>
        public int MaxAge { get; set; }

        #region Overrides of ActionFilterAttribute

        /// <summary>
        /// Set HTTP cache parameters.
        /// </summary>
        /// <param name="filterContext">The filter context.</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            if (!filterContext.IsChildAction)
            {
                var cache = filterContext.HttpContext.Response.Cache;
                cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                cache.SetCacheability(HttpCacheability.ServerAndPrivate);
                cache.SetMaxAge(TimeSpan.FromSeconds(MaxAge));
                cache.SetExpires(DateTime.UtcNow.AddSeconds(MaxAge > 0 ? MaxAge - 1 : MaxAge));
            }
            base.OnResultExecuted(filterContext);
        }

        #endregion
    }
}