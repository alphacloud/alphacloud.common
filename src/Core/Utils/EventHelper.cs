using System;
using AccountView.Common.Base.Annotations;

namespace AccountView.Common.Base.Data
{
    /// <summary>
    ///     Event helper.
    /// </summary>
    public class EventHelper
    {
        /// <summary>
        ///     Raise event if subscribers exist.
        /// </summary>
        /// <typeparam name="TEventArgs">Event arguments type.</typeparam>
        /// <param name="handler">Event handler.</param>
        /// <param name="eventArgs">event arguments</param>
        /// <param name="sender">sender</param>
        public static void Raise<TEventArgs>(EventHandler<TEventArgs> handler, [NotNull] TEventArgs eventArgs,
            [CanBeNull] object sender = null)
            where TEventArgs : EventArgs
        {
            if (eventArgs == null) throw new ArgumentNullException("eventArgs");
            if (handler != null)
                handler(sender, eventArgs);
        }

        /// <summary>
        ///     Check
        /// </summary>
        /// <typeparam name="TEventArgs">Event arguments type.</typeparam>
        /// <param name="handler">Event handler.</param>
        /// <param name="eventArgsBuilder">Event arguments builder. WIll be called if event subscribers exist.</param>
        /// <param name="sender">sender</param>
        public static void Raise<TEventArgs>(EventHandler<TEventArgs> handler,
            [NotNull] Func<TEventArgs> eventArgsBuilder,
            [CanBeNull] object sender = null)
            where TEventArgs : EventArgs
        {
            if (eventArgsBuilder == null) throw new ArgumentNullException("eventArgsBuilder");
            if (handler != null)
            {
                TEventArgs eventArgs = eventArgsBuilder();
                if (eventArgs == null)
                    throw new InvalidOperationException("EventArgs builder returned null");
                handler(sender, eventArgs);
            }
        }
    }
}