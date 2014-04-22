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
    using System.Collections.Concurrent;
    using System.Web;
    using System.Web.Mvc;
    using JetBrains.Annotations;


    /// <summary>
    ///   Disable client-side cache for request.
    /// </summary>
    /// <remarks>If both this and <see cref="CachePrivatelyAttribute" /> applied, latest has higher priority.</remarks>
    [PublicAPI]
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public sealed class DisableClientCacheAttribute : ActionFilterAttribute
    {
        const string FilterAppliedMarkerKey = "DisableClientCacheFilter.Applied";

        static readonly ConcurrentDictionary<string, bool> s_cacheOverrides = new
            ConcurrentDictionary<string, bool>(8, 100);


        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.IsChildAction)
            {
                return;
            }

            if (filterContext.HttpContext.Items[FilterAppliedMarkerKey] != null)
            {
                return;
            }
            var response = filterContext.RequestContext.HttpContext.Response;

            var key = filterContext.ActionDescriptor.UniqueId;

            if (s_cacheOverrides.GetOrAdd(key, k => CheckOverrides(filterContext)))
            {
                return;
            }

            response.Cache.SetCacheability(HttpCacheability.ServerAndNoCache);
            response.Cache.SetMaxAge(TimeSpan.Zero);
            response.Cache.SetExpires(DateTime.UtcNow.AddSeconds(-1));
            response.Cache.SetNoStore();

            filterContext.HttpContext.Items[FilterAppliedMarkerKey] =
                string.Concat(filterContext.ActionDescriptor.ControllerDescriptor.ControllerName, ".",
                    filterContext.ActionDescriptor.ActionName);
        }


        static bool CheckOverrides(ActionExecutedContext filterContext)
        {
            var action = filterContext.ActionDescriptor;
            if (action.IsDefined(typeof (CachePrivatelyAttribute), true))
            {
                return true;
            }

            var controller = action.ControllerDescriptor;
            if (controller.IsDefined(typeof (CachePrivatelyAttribute), true))
            {
                return true;
            }
            return false;
        }
    }
}