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

namespace Alphacloud.Common.Infrastructure.Instrumentation
{
    using System;
    using System.Collections.Generic;
    using Core.Data;
    using JetBrains.Annotations;

    /// <summary>
    ///   Instrumentation logging configuration.
    /// </summary>
    [Serializable]
    public class LoggingConfiguration
    {
        const string DefaultSettingsKey = "Default";
        readonly IDictionary<string, DurationSettings> _durationSettings;


        public LoggingConfiguration()
        {
            _durationSettings = new Dictionary<string, DurationSettings> {
                {
                    DefaultSettingsKey,
                    new DurationSettings {
                        Enabled = false,
                        InfoLevelThreshold = TimeSpan.FromMilliseconds(500),
                        WarningLevelThreshold = TimeSpan.FromSeconds(5)
                    }
                }
            };

            OperationLogging = new CallCounterSettings {
                Enabled = false,
                InfoLevelThreshold = 6,
                WarningLevelThreshold = 12
            };

            LogDuplicateCalls = false;
        }


        /// <summary>
        ///   Operation logging settings.
        /// </summary>
        public CallCounterSettings OperationLogging { get; private set; }

        /// <summary>
        ///   Should duplicate service or database calls be logged.
        /// </summary>
        public bool LogDuplicateCalls { get; set; }


        public DurationSettings GetCallDurationSettings(string callType = null)
        {
            callType = string.IsNullOrEmpty(callType) ? DefaultSettingsKey : callType;
            var settings = _durationSettings.ValueOrDefault(callType);
            return settings ?? _durationSettings[DefaultSettingsKey];
        }


        public void AddDurationSettings([NotNull] string callType, [NotNull] DurationSettings settings)
        {
            if (settings == null) throw new ArgumentNullException("settings");
            if (string.IsNullOrEmpty(callType)) throw new ArgumentNullException("callType");

            _durationSettings.Add(callType, settings);
        }

        #region Nested type: CallCounterSettings

        /// <summary>
        ///   Call count-based setting.
        ///   Log level is determined based on call-count.
        /// </summary>
        [Serializable]
        public class CallCounterSettings
        {
            public bool Enabled { get; internal set; }
            public int InfoLevelThreshold { get; internal set; }
            public int WarningLevelThreshold { get; internal set; }
        }

        #endregion

        #region Nested type: DurationSettings

        /// <summary>
        ///   Duration-based settings.
        ///   Log-level is determined based on total operation duration.
        /// </summary>
        [Serializable]
        public class DurationSettings
        {
            public bool Enabled { get; internal set; }
            public TimeSpan InfoLevelThreshold { get; internal set; }
            public TimeSpan WarningLevelThreshold { get; internal set; }
        }

        #endregion
    }
}