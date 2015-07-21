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
    ///   Configuration section for predefined cache durations.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class CacheDurationSection : ConfigurationSection
    {
        /// <summary>
        ///   Tiny (intra-request) cache ttl.
        /// </summary>
        [ConfigurationProperty("tiny", DefaultValue = "00:00:30", IsRequired = true)]
        public TimeSpan Tiny
        {
            get { return (TimeSpan) this["tiny"]; }
            set { this["tiny"] = value; }
        }

        /// <summary>
        ///   Short (per-request) cache ttl.
        /// </summary>
        [ConfigurationProperty("short", DefaultValue = "00:01:00", IsRequired = true)]
        public TimeSpan Short
        {
            get { return (TimeSpan) this["short"]; }
            set { this["short"] = value; }
        }

        /// <summary>
        ///   Balanced cache ttl.
        /// </summary>
        [ConfigurationProperty("balanced", DefaultValue = "00:05:00", IsRequired = true)]
        public TimeSpan Balanced
        {
            get { return (TimeSpan) this["balanced"]; }
            set { this["balanced"] = value; }
        }

        /// <summary>
        ///   Long (rarely modified data) cache ttl.
        /// </summary>
        [ConfigurationProperty("long", DefaultValue="00:30:00", IsRequired = true)]
        public TimeSpan Long
        {
            get { return (TimeSpan) this["long"]; }
            set { this["long"] = value; }
        }

        /// <summary>
        ///   Huge (static, expensive, data with manually controlled caching) cache ttl.
        /// </summary>
        [ConfigurationProperty("huge", DefaultValue = "01:00:00", IsRequired = false)]
        public TimeSpan Huge
        {
            get { return (TimeSpan) this["huge"]; }
            set { this["huge"] = value; }
        }
    }
}