namespace Alphacloud.Common.Infrastructure.Instrumentation
{
    using System;
    using System.Globalization;
    using System.Security.Principal;
    using System.Threading;
    using Core.Instrumentation;
    using Core.Utils;
    using global::Common.Logging;

    /// <summary>
    ///   Stores captured instrumentation context.
    ///   Context includes:
    ///   <list type="table">
    ///     <item>
    ///       <term>Culture</term>
    ///       <description>
    ///         <see cref="Thread.CurrentCulture" />
    ///       </description>
    ///     </item>
    ///     <item>
    ///       <term>UI Culture</term>
    ///       <description>
    ///         <see cref="Thread.CurrentUICulture" />
    ///       </description>
    ///     </item>
    ///     <item>
    ///       <term>Principal</term>
    ///       <description>
    ///         <see cref="Thread.CurrentPrincipal" />
    ///       </description>
    ///     </item>
    ///     <item>
    ///       <term>Correlation Id</term>
    ///       <description>Correlation Id</description>
    ///     </item>
    ///     <item>
    ///       <term>Instrumentation context</term>
    ///       <description>
    ///         <see cref="IInstrumentationContext" />
    ///       </description>
    ///     </item>
    ///   </list>
    /// </summary>
    /// Instrumentation context and correlation Id captuted only if instrumentation is enabled.
    public class CapturedContext
    {
        static readonly ILog s_log = LogManager.GetCurrentClassLogger();
        static readonly IDisposable s_emptyContext = new EmptyContext();

        readonly string _correlationId;
        readonly CultureInfo _culture;
        readonly IInstrumentationContext _instrumentationContext;
        readonly int _originThread;
        readonly IPrincipal _principal;
        readonly CultureInfo _uiCulture;
        readonly bool _instrumentationEnabled;


        internal CapturedContext(
            CultureInfo culture, CultureInfo uiCulture,
            IPrincipal principal,
            int originThread,
            string correlationId, IInstrumentationContext instrumentationContext)
        {
            _culture = culture;
            _uiCulture = uiCulture;
            _principal = principal;
            _correlationId = correlationId;
            _instrumentationContext = instrumentationContext;
            _originThread = originThread;
            _instrumentationEnabled = true;
        }

        internal CapturedContext(
            CultureInfo culture, CultureInfo uiCulture,
            IPrincipal principal,
            int originThread)
        {
            _culture = culture;
            _uiCulture = uiCulture;
            _principal = principal;
            _originThread = originThread;
            _instrumentationEnabled = false;
            _correlationId = null;
            _instrumentationContext = null;
        }


        /// <summary>
        ///   Set captured context on current thread.
        /// </summary>
        /// <returns>Saved context handler (<see cref="IDisposable" />) used to restore old thread parameters.</returns>
        public IDisposable Set()
        {
            var oldCulture = Thread.CurrentThread.CurrentCulture;
            var oldUiCulture = Thread.CurrentThread.CurrentUICulture;
            var oldPrincipal = Thread.CurrentPrincipal;

            var curThread = Thread.CurrentThread;
            var currentTid = curThread.ManagedThreadId;

            if (_originThread == currentTid)
            {
                s_log.Debug("Using origin thread - nothing to set");
                return s_emptyContext;
            }

            SetOnCurrentThread(_culture, _uiCulture, _principal);

            var scope = s_emptyContext;
            if (_instrumentationEnabled)
            {
                scope = SafeServiceLocator.Resolve<ICorrelationIdProvider>().SetId(_correlationId);
                SafeServiceLocator.Resolve<IInstrumentationContextProvider>()
                    .SetInstrumentationContext(_instrumentationContext);
            }
            s_log.DebugFormat("Set captured context of thread {0}", _originThread);
            return new SavedContext(oldCulture, oldUiCulture, oldPrincipal, currentTid, scope);
        }


        static void SetOnCurrentThread(CultureInfo culture, CultureInfo uiCulture, IPrincipal principal)
        {
            var curThread = Thread.CurrentThread;
            curThread.CurrentCulture = culture;
            curThread.CurrentUICulture = uiCulture;
            Thread.CurrentPrincipal = principal;
        }

        #region Nested type: EmptyContext

        class EmptyContext : IDisposable
        {
            #region IDisposable Members

            public void Dispose()
            {
                s_log.Debug("Same thread - nothing to dispose.");
            }

            #endregion
        }

        #endregion

        #region Nested type: SavedContext

        /// <summary>
        ///   Saved context, will be restored when disposed.
        /// </summary>
        class SavedContext : IDisposable
        {
            readonly int _managedThreadId;
            readonly CultureInfo _oldCulture;
            readonly IPrincipal _oldPrincipal;
            readonly CultureInfo _oldUiCulture;
            readonly IDisposable _scope;


            internal SavedContext(CultureInfo oldCulture, CultureInfo oldUiCulture, IPrincipal oldPrincipal,
                int managedThreadId, IDisposable scope)
            {
                _oldCulture = oldCulture;
                _oldUiCulture = oldUiCulture;
                _oldPrincipal = oldPrincipal;
                _managedThreadId = managedThreadId;
                _scope = scope;
            }

            #region IDisposable Members

            /// <summary>
            ///   Restores saved context
            /// </summary>
            public void Dispose()
            {
                if (Thread.CurrentThread.ManagedThreadId != _managedThreadId)
                {
                    s_log.WarnFormat("Attempt to restore context saved for threadd {0} on thread {1}. Ignored",
                        _managedThreadId, Thread.CurrentThread.ManagedThreadId);
                    return;
                }
                SetOnCurrentThread(_oldCulture, _oldUiCulture, _oldPrincipal);
                Disposer.TryDispose(_scope);
                s_log.DebugFormat("Restored context for thread {0}", _managedThreadId);
            }

            #endregion
        }

        #endregion
    }
}