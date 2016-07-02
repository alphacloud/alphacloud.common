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

namespace Alphacloud.Common.Infrastructure.Caching
{
    using System;
    using System.Configuration;
    using Core.Data;
    using JetBrains.Annotations;
    using global::Common.Logging;

    /// <summary>
    ///   Represents predefined cache durations based on config file settings.
    /// </summary>
    [PublicAPI]
    public class CacheDuration
    {
        const string ConfigPath = "alphacloud/cache/duration";
        static readonly Lazy<CacheDuration> s_duration = new Lazy<CacheDuration>(LoadDurationSettings);


        static readonly ILog s_log = LogManager.GetLogger<CacheDuration>();


        /// <summary>
        ///   Initialize with default settings.
        /// </summary>
        protected internal CacheDuration()
        {
            Tiny = TimeSpan.FromSeconds(10);
            Short = TimeSpan.FromMinutes(1);
            Balanced = TimeSpan.FromMinutes(5);
            Long = TimeSpan.FromMinutes(30);
            Huge = TimeSpan.FromHours(1);
        }


        /// <summary>
        ///   Tiny cache duration.
        ///   Use for intra-request caching.
        /// </summary>
        public TimeSpan Tiny { get; set; }

        /// <summary>
        ///   Gets the Short cache duration.
        ///   Use for per-request cache.
        /// </summary>
        /// <value>
        ///   Cache duration timeout.
        /// </value>
        public TimeSpan Short { get; private set; }

        /// <summary>
        ///   Gets the Balanced cache duration.
        /// </summary>
        /// <value>
        ///   Cache duration timeout.
        /// </value>
        public TimeSpan Balanced { get; private set; }

        /// <summary>
        ///   Gets the Long cache duration.
        ///   User for caching rarely changed data.
        /// </summary>
        /// <value>
        ///   Cache duration timeout.
        /// </value>
        public TimeSpan Long { get; private set; }

        /// <summary>
        ///   Gets the Huge cache duration.
        ///   User for static data or data which is hard to retrieve.
        /// </summary>
        /// <value>
        ///   Cache duration timeout.
        /// </value>
        public TimeSpan Huge { get; private set; }


        /// <summary>
        ///   Gets the instance (singleton).
        /// </summary>
        public static CacheDuration Instance
        {
            get { return s_duration.Value; }
        }


        internal static CacheDuration LoadDurationSettings()
        {
            var duration = new CacheDuration();
            var section = (CacheDurationSection) ConfigurationManager.GetSection(ConfigPath);
            if (section == null)
            {
                s_log.WarnFormat("Configuration section '{0}' not found, using default settings ({1})",
                    ConfigPath, duration);
            }
            else
            {
                duration.Tiny = section.Tiny;
                duration.Short = section.Short;
                duration.Balanced = section.Balanced;
                duration.Long = section.Long;
                duration.Huge = section.Huge;

                s_log.Info(m => m("Loaded from '{0}', setings: {1}", ConfigPath, duration));
            }
            return duration;
        }


        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return "{0:0.0} secs; {1:0.0}; {2:0.0}; {3:0.0} and {4:0.0} minutes".ApplyArgs(Tiny.TotalSeconds,
                Short.TotalMinutes, Balanced.TotalMinutes,
                Long.TotalMinutes, Huge.TotalMinutes);
        }
    }
}