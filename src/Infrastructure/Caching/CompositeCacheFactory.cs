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
    using System.Collections.Specialized;
    using System.Configuration;
    using System.Globalization;

    using JetBrains.Annotations;

    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    ///   Created composite (2 level cache)
    /// </summary>
    [UsedImplicitly]
    public class CompositeCacheFactory : CacheFactoryBase
    {
        /// <summary>
        ///   The default local cache timeout in seconds.
        /// </summary>
        const int DefaultLocalCacheTimeout = 5;

        string _defaultCacheName;
        bool _devMode;
        ICacheFactory _localCacheFactory;
        TimeSpan _localCacheTimeout = TimeSpan.FromSeconds(DefaultLocalCacheTimeout);
        ICacheFactory _remoteCacheFactory;


        /// <summary>
        ///   Initializes a new instance of the <see cref="CompositeCacheFactory" /> class.
        /// </summary>
        public CompositeCacheFactory()
        {
            var section = (NameValueCollection) ConfigurationManager.GetSection("alphacloud/cache/parameters");
            if (section != null)
                LoadFromConfig(section);
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="CompositeCacheFactory" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <exception cref="System.ArgumentNullException">settings</exception>
        public CompositeCacheFactory([NotNull] NameValueCollection settings) : base(settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            LoadFromConfig(settings);
        }


        protected override ICache CreateCache(string instance)
        {
            var cache = base.CreateCache(instance);
            if (cache != null)
                return cache;

            if (_localCacheFactory == null || _remoteCacheFactory == null)
                throw new InvalidOperationException("CacheFactory was not initialized");

            instance = string.IsNullOrEmpty(instance) ? _defaultCacheName : instance;

            var localCacheTimeout = GetLocalTimeoutStrategy();

            return new CompositeCache(
                _localCacheFactory.GetCache(instance),
                _remoteCacheFactory.GetCache(instance),
                _localCacheTimeout, _devMode);
        }

        ILocalCacheTimeoutStrategy GetLocalTimeoutStrategy()
        {
            try
            {
                return ServiceLocator.Current.GetInstance<ILocalCacheTimeoutStrategy>();
            }
            catch (Exception ex)
            {
                Log.WarnFormat(
                    "Cannot resolve local cache timeout strategy provider, using fixed {0} seconds. Ensure '{0}' is registered in IOC",
                    ex,
                    typeof (ILocalCacheTimeoutStrategy).Name);
                return new FixedTimeoutStrategy(TimeSpan.FromSeconds(DefaultLocalCacheTimeout));
            }
        }


        public override void Initialize()
        {
            if (IsEnabled)
            {
                _remoteCacheFactory = ServiceLocator.Current.GetInstance<ICacheFactory>("remote");
                _localCacheFactory = ServiceLocator.Current.GetInstance<ICacheFactory>("local");

                _remoteCacheFactory.Initialize();
                _localCacheFactory.Initialize();
            }
        }


        void LoadFromConfig(NameValueCollection settings)
        {
            if (!TimeSpan.TryParse(settings["localCacheTimeout"], CultureInfo.InvariantCulture, out _localCacheTimeout))
            {
                _localCacheTimeout = TimeSpan.FromSeconds(DefaultLocalCacheTimeout);
            }
            _defaultCacheName = settings["cacheName"];
            bool.TryParse(settings["devMode"], out _devMode);
        }
    }
}
