#region copyright

// Copyright 2014 Alphacloud.Net
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
    using global::Common.Logging;
    using JetBrains.Annotations;

    /// <summary>
    ///   Instrumentation and correlation management runtime.
    /// </summary>
    /// <remarks>
    ///   Requires following services to be registered in ServiceLocator:
    ///   <list type="bullet">
    ///     <item>
    ///       <term>
    ///         <see cref="ICorrelationIdProvider" />
    ///       </term>
    ///       <description>Correlation ID provider.</description>
    ///     </item>
    ///     <item>
    ///       <term>
    ///         <see cref="IInstrumentationContextProvider" />
    ///       </term>
    ///       <description>InstrumentationContext provider.</description>
    ///     </item>
    ///   </list>
    /// </remarks>
    [PublicAPI]
    public class InstrumentationRuntime
    {
        static readonly ILog s_log = LogManager.GetCurrentClassLogger();

        static Lazy<InstrumentationRuntime> s_instance =
            new Lazy<InstrumentationRuntime>(() => new InstrumentationRuntime());

        static readonly InstrumentationSettings s_defaultSettings = new InstrumentationSettings();


        Func<InstrumentationSettings> _configurationProvider;
        readonly Lazy<ICorrelationIdProvider> _correlationIdProvider = new Lazy<ICorrelationIdProvider>(SafeServiceLocator.Resolve<ICorrelationIdProvider>);
        readonly Lazy<IInstrumentationContextProvider> _instrumentationContextProvider = new Lazy<IInstrumentationContextProvider>(SafeServiceLocator.Resolve<IInstrumentationContextProvider>);

        /// <summary>
        ///   Instrumentation runtime instance.
        /// </summary>
        public static InstrumentationRuntime Instance
        {
            get { return s_instance.Value; }
        }

        /// <summary>
        ///   The database call completed event.
        /// </summary>
        public event EventHandler<InstrumentationEventArgs> DatabaseCallCompleted;

        /// <summary>
        ///   The operation completed event.
        /// </summary>
        public event EventHandler<OperationCompletedEventArgs> OperationCompleted;

        /// <summary>
        ///   The service call completed event.
        /// </summary>
        public event EventHandler<InstrumentationEventArgs> ServiceCallCompleted;


        /// <summary>
        ///   Attach logger.
        /// </summary>
        /// <param name="listener">Logger.</param>
        public void Attach([NotNull] IInstrumentationEventListener listener)
        {
            if (listener == null) throw new ArgumentNullException("listener");

            DatabaseCallCompleted += listener.DatabaseCallCompleted;
            ServiceCallCompleted += listener.ServiceCallCompleted;
            OperationCompleted += listener.OperationCompleted;
        }


        /// <summary>
        ///   Detach logger.
        /// </summary>
        /// <param name="listener">Logger.</param>
        public void Detach([NotNull] IInstrumentationEventListener listener)
        {
            if (listener == null) throw new ArgumentNullException("listener");

            DatabaseCallCompleted -= listener.DatabaseCallCompleted;
            ServiceCallCompleted -= listener.ServiceCallCompleted;
            OperationCompleted -= listener.OperationCompleted;
        }


        /// <summary>
        ///   Sets instrumentation settings provider.
        /// </summary>
        /// <param name="configurationProvider">Settings provider.</param>
        /// <remarks>
        ///   Settings returned by provider are not cached by instrumentation runtime.
        /// </remarks>
        public void SetConfigurationProvider([NotNull] Func<InstrumentationSettings> configurationProvider)
        {
            if (configurationProvider == null) throw new ArgumentNullException("configurationProvider");
            _configurationProvider = configurationProvider;
        }


        /// <summary>
        /// Logs operation completion and broadcasts <see cref="OperationCompletedEventArgs"/> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="operation">The operation.</param>
        /// <param name="duration">The duration.</param>
        public void OnOperationCompleted([CanBeNull] object sender, string operation, TimeSpan duration)
        {
            if (!IsEnabled())
                return;

            EventHelper.Raise(OperationCompleted, () => new OperationCompletedEventArgs {
                Context = _instrumentationContextProvider.Value.GetInstrumentationContext(),
                CorrelationId = _correlationIdProvider.Value.GetId(),
                Duration = duration,
                Command = operation,
                ManagedThreadId = Thread.CurrentThread.ManagedThreadId
            }, sender);
        }


        /// <summary>
        ///   Logs service call completion and broadcasts <see cref="ServiceCallCompleted" /> event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="methodName">Service method name.</param>
        /// <param name="duration">Call duration.</param>
        public void OnServiceCallCompleted([CanBeNull] object sender, string methodName, TimeSpan duration)
        {
            if (!IsEnabled())
                return;

            _instrumentationContextProvider.Value.GetInstrumentationContext().AddServiceCall(methodName, duration);

            EventHelper.Raise(ServiceCallCompleted, () => new InstrumentationEventArgs {
                CorrelationId = _correlationIdProvider.Value.GetId(),
                Duration = duration,
                ManagedThreadId = Thread.CurrentThread.ManagedThreadId,
                Command = methodName
            }, sender);
        }


        /// <summary>
        ///   Logs database call completions and broadcasts <see cref="DatabaseCallCompleted" /> event.
        /// </summary>
        /// <param name="sender">Sender.</param>
        /// <param name="sql">Database call information.</param>
        /// <param name="duration">Duration.</param>
        public void OnDatabaseCallCompleted([CanBeNull] object sender, string sql, TimeSpan duration)
        {
            if (!IsEnabled())
                return;

            _instrumentationContextProvider.Value.GetInstrumentationContext().AddDatabaseCall(sql, duration);

            EventHelper.Raise(DatabaseCallCompleted, () => new InstrumentationEventArgs {
                CorrelationId = _correlationIdProvider.Value.GetId(),
                Duration = duration,
                ManagedThreadId = Thread.CurrentThread.ManagedThreadId,
                Command = sql
            }, sender);
        }


        /// <summary>
        ///   Captures instrumentation context.
        /// </summary>
        /// <returns>Captured execution context.</returns>
        /// <remarks>
        ///   If instrumentation is not enabled, culture info (<see cref="Thread.CurrentCulture" />,
        ///   <see cref="Thread.CurrentUICulture" />)
        ///   and Principal (<see cref="Thread.CurrentPrincipal" />.
        /// </remarks>
        public CapturedContext CaptureContext()
        {
            var currentThread = Thread.CurrentThread;
            var culture = currentThread.CurrentCulture;
            var uiCulture = currentThread.CurrentUICulture;
            var principal = Thread.CurrentPrincipal;

            CapturedContext ctx;

            if (IsEnabled())
            {
                var correlationId = _correlationIdProvider.Value.GetId();
                var instrumentationContext = _instrumentationContextProvider.Value.GetInstrumentationContext();
                ctx = new CapturedContext(culture, uiCulture, principal, currentThread.ManagedThreadId, correlationId,
                    instrumentationContext);
            }
            else
                ctx = new CapturedContext(culture, uiCulture, principal, currentThread.ManagedThreadId);

            s_log.Debug("Captured execution context");

            return ctx;
        }


        /// <summary>
        ///   Reset runtime.
        ///   For Debug / Testing purposes only.
        /// </summary>
        internal static void Reset()
        {
            s_instance = new Lazy<InstrumentationRuntime>(() => new InstrumentationRuntime());
        }


        [NotNull]
        InstrumentationSettings GetConfiguration()
        {
            return _configurationProvider != null
                ? _configurationProvider()
                : s_defaultSettings;
        }


        bool IsEnabled()
        {
            return GetConfiguration().Enabled;
        }
    }
}