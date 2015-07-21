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
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Configuration;
    using JetBrains.Annotations;
    using global::Common.Logging;

    /// <summary>
    ///   Base class for cache factory.
    /// </summary>
    [PublicAPI]
    public abstract class CacheFactoryBase : ICacheFactory, IDisposable
    {
        /// <summary>
        ///   Default cache instance name.
        /// </summary>
        public const string DefaultInstanceName = "default";

        const string CacheParametersConfigPath = "alphacloud/cache/parameters";
        readonly IDictionary<string, ICache> _caches = new SortedList<string, ICache>(16);
        readonly ILog _log;


        /// <summary>
        ///   Initializes a new instance of the <see cref="CacheFactoryBase" /> class.
        /// </summary>
        protected CacheFactoryBase()
        {
            _log = LogManager.GetLogger(GetType());
            IsEnabled = true;
            var section = (NameValueCollection) ConfigurationManager.GetSection(CacheParametersConfigPath);
            if (section != null)
                LoadParameters(section);
        }


        /// <summary>
        ///   Initializes factory with custom configuration settings.
        /// </summary>
        /// <param name="settings">Configuration settings.</param>
        /// <exception cref="System.ArgumentNullException">settings is null</exception>
        protected CacheFactoryBase([NotNull] NameValueCollection settings)
        {
            _log = LogManager.GetLogger(GetType());
            IsEnabled = true;

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            LoadParameters(settings);
        }


        /// <summary>
        ///   Logger
        /// </summary>
        protected ILog Log
        {
            get { return _log; }
        }

        #region Implementation of ICacheFactory

        /// <summary>
        ///   Gets a value indicating whether caching is enabled.
        /// </summary>
        public bool IsEnabled { get; private set; }


        /// <summary>
        ///   Get cache instance.
        /// </summary>
        /// <param name="instance">Instance name (optional)</param>
        /// <returns>
        ///   Cache instance
        /// </returns>
        public ICache GetCache(string instance = null)
        {
            string instanceName = instance ?? DefaultInstanceName;
            Log.DebugFormat("Resolving cache '{0}'", instanceName);
            CheckDisposed();

            ICache cache;
            lock (_caches)
            {
                if (!_caches.TryGetValue(instanceName, out cache))
                {
                    cache = CreateCache(instance);
                    Log.DebugFormat("Created new instance of cache '{0}'", instanceName);
                    _caches[instanceName] = cache;
                }
            }

            Log.InfoFormat("Resolved cache '{0}'", instanceName);
            return cache;
        }


        /// <summary>
        ///   Initialize cache factory.
        /// </summary>
        public abstract void Initialize();


        /// <summary>
        ///   Check wether factory is disposed.
        /// </summary>
        /// <exception cref="ObjectDisposedException">if factory is disposed.</exception>
        protected void CheckDisposed()
        {
            if (IsDisposed)
            {
                throw new ObjectDisposedException("CacheFactory");
            }
        }


        /// <summary>
        ///   Create named cache instance.
        /// </summary>
        /// <param name="instance">Instance name.</param>
        /// <returns>
        ///   <see cref="ICache" /> instance or <see cref="CacheBase.Null" /> null cache if caching is disabled.
        /// </returns>
        protected internal virtual ICache CreateCache(string instance)
        {
            return IsEnabled ? null : CacheBase.Null;
        }

        #endregion

        #region Implementation of IDisposable

        /// <summary>
        ///   Gets a value indicating whether this instance is disposed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is disposed; otherwise, <c>false</c>.
        /// </value>
        public bool IsDisposed { get; private set; }


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }


        protected virtual void Dispose(bool disposing)
        {
            if (!disposing || IsDisposed)
            {
                return;
            }

            lock (_caches)
            {
                Log.Debug(m => m("Disposing {0} caches...", _caches.Count));
                foreach (var kvp in _caches)
                {
                    var c = kvp.Value as IDisposable;
                    if (c == null)
                    {
                        continue;
                    }
                    Log.DebugFormat("Disposing cache '{0}' ...", kvp.Key);
                    c.Dispose();
                }
                _caches.Clear();
                IsDisposed = true;
                Log.Info("Disposed factory and caches.");
            }
        }

        #endregion

        void LoadParameters(NameValueCollection section)
        {
            bool isEnabled;
            var enabled = section != null ? section["enabled"] : bool.TrueString;
            if (bool.TryParse(enabled, out isEnabled))
            {
                IsEnabled = isEnabled;
            }
            if (!IsEnabled)
            {
                Log.Warn("Cache is disabled in configuration, using NullCache");
            }
        }
    }
}