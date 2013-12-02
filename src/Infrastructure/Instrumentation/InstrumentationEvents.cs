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

namespace Alphacloud.Common.Infrastructure.Instrumentation
{
    using System;
    using System.Threading;
    using Core.Instrumentation;
    using Core.Utils;
    using JetBrains.Annotations;

    public class InstrumentationEventArgs : EventArgs
    {
        public int ManagedThreadId { get; set; }

        public string OperationName { get; set; }

        public TimeSpan Duration { get; set; }
    }


    public class OperationCompletedEventArgs : InstrumentationEventArgs
    {
        public IInstrumentationContext Context { get; set; }
    }

    public class InstrumentationSettings
    {
        public bool Enabled { get; set; }
    }

    public class InstrumentationEvents
    {
        static readonly InstrumentationEvents s_instance = new InstrumentationEvents();

        readonly InstrumentationSettings _defaultSettings = new InstrumentationSettings();
        readonly Lazy<IInstrumentationContextProvider> _instrumentationContextProvider;
        Func<InstrumentationSettings> _configurationProvider;

        public EventHandler<InstrumentationEventArgs> DatabaseCallCompleted;

        public EventHandler<OperationCompletedEventArgs> OperationCompleted;

        public EventHandler<InstrumentationEventArgs> ServiceCallCompleted;


        public static InstrumentationEvents Instance
        {
            get { return s_instance; }
        }

        InstrumentationEvents()
        {
            _instrumentationContextProvider = new Lazy<IInstrumentationContextProvider>(SafeServiceLocator.Resolve<IInstrumentationContextProvider>);
        }


        public void OnOperationCompleted(object sender, string operation, TimeSpan duration)
        {
            if (!GetConfiguration().Enabled)
                return;

            var context = _instrumentationContextProvider.Value.GetInstrumentationContext();
            EventHelper.Raise(OperationCompleted, () => new OperationCompletedEventArgs {
                Context = context,
                Duration = duration,
                OperationName = operation,
                ManagedThreadId = Thread.CurrentThread.ManagedThreadId
            }, sender);
        }


        public void OnServiceCallCompleted(object sender, string methodName, TimeSpan duration)
        {
            if (!GetConfiguration().Enabled)
                return;

            var context = _instrumentationContextProvider.Value.GetInstrumentationContext();
            context.AddServiceCall(methodName, duration);

            EventHelper.Raise(ServiceCallCompleted, () => new InstrumentationEventArgs {
                Duration = duration,
                ManagedThreadId = Thread.CurrentThread.ManagedThreadId,
                OperationName = methodName
            }, sender);
        }


        public void OnDatabaseCallCompleted(object sender, string sql, TimeSpan duration)
        {
            if (!GetConfiguration().Enabled)
                return;

            var context = _instrumentationContextProvider.Value.GetInstrumentationContext();
            context.AddDatabaseCall(sql, duration);

            EventHelper.Raise(DatabaseCallCompleted, () => new InstrumentationEventArgs {
                Duration = duration,
                ManagedThreadId = Thread.CurrentThread.ManagedThreadId,
                OperationName = sql
            });
        }


        public void SetConfigurationProvider([NotNull] Func<InstrumentationSettings> settingsProvider)
        {
            if (settingsProvider == null) throw new ArgumentNullException("settingsProvider");
            _configurationProvider = settingsProvider;
        }


        [NotNull]
        InstrumentationSettings GetConfiguration()
        {
            return _configurationProvider != null
                ? _configurationProvider()
                : _defaultSettings;
        }
    }
}