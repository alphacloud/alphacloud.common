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

namespace Alphacloud.Common.Caching.Memcached
{
    using Infrastructure.Caching;
    using global::Common.Logging;

    /// <summary>
    ///   Memcached availability monitor.
    ///   Since memcached client handles cache host unavailability gracefully, no need to perform additional checks here.
    /// </summary>
    class MemcachedAvailabilityMonitor : ICacheHealthcheckMonitor
    {
        static readonly ILog s_log = LogManager.GetCurrentClassLogger();

        #region ICacheHealthcheckMonitor Members

        public bool IsCacheAvailable
        {
            get { return true; }
        }


        public void Start()
        {
            s_log.Info("Start cache healthcheck");
        }

        #endregion
    }
}