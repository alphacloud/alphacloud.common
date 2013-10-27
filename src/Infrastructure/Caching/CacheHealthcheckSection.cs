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
    using System.Diagnostics.CodeAnalysis;
    using JetBrains.Annotations;

    /// <summary>
    /// Cache healthcheck configuration section.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CacheHealthcheckSection : ConfigurationSection
    {
        /// <summary>
        /// Gets or sets the connection timeout.
        /// </summary>
        /// <value>
        /// The connection timeout.
        /// </value>
        [ConfigurationProperty("connectionTimeout", DefaultValue = "00:00:15")]
        public TimeSpan ConnectionTimeout
        {
            get { return (TimeSpan) this["connectionTimeout"]; }
            set { this["connectionTimeout"] = value; }
        }

        /// <summary>
        /// Gets or sets the healthcheck interval.
        /// </summary>
        /// <value>
        /// The healthcheck interval.
        /// </value>
        [ConfigurationProperty("interval", DefaultValue = "00:01:00")]
        public TimeSpan HealthcheckInterval
        {
            get { return (TimeSpan) this["interval"]; }
            set { this["interval"] = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether healthcheck is enabled.
        /// </summary>
        /// <value>
        /// <c>true</c> if healthckech is enabled; otherwise, <c>false</c>.
        /// </value>
        [ConfigurationProperty("enabled", DefaultValue = true)]
        public bool IsEnabled
        {
            get { return (bool) this["enabled"]; }
            set { this["enabled"] = value; }
        }
    }
}
