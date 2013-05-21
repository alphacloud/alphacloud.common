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

namespace Alphacloud.Common.Core.Utils
{
    using System;
    using JetBrains.Annotations;

    /// <summary>
    ///   Event helper.
    /// </summary>
    [PublicAPI]
    public class EventHelper
    {
        /// <summary>
        ///   Raise event if subscribers exist.
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
        ///   Raise event if subscribers exist.
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
            if (handler == null) return;
            var eventArgs = eventArgsBuilder();
            if (eventArgs == null)
                throw new InvalidOperationException("EventArgs builder returned null");
            handler(sender, eventArgs);
        }
    }
}