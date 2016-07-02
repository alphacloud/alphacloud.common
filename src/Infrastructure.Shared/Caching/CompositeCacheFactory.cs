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

namespace Alphacloud.Common.Infrastructure.Caching
{
    using System;
    using System.Collections.Specialized;
    using System.Configuration;
    using JetBrains.Annotations;


    /// <summary>
    ///   Composite cache factory (Two Level Cache).
    /// </summary>
    [UsedImplicitly]
    public class CompositeCacheFactory : CacheFactoryBase
    {
        const string CacheParametersSectionName = "alphacloud/cache/parameters";
        readonly ICacheFactory _backingCacheFactory;
        readonly ICacheFactory _localCacheFactory;
        readonly ILocalCacheTimeoutStrategy _localCacheTimeout;

        string _defaultCacheName;
        bool _devMode;


        /// <summary>
        ///   Initializes a new instance of the <see cref="CompositeCacheFactory" /> class.
        /// </summary>
        public CompositeCacheFactory([NotNull] ILocalCacheTimeoutStrategy localCacheTimeout,
            [NotNull] ICacheFactory localCacheFactory, [NotNull] ICacheFactory backingCacheFactory)
        {
            if (localCacheTimeout == null) throw new ArgumentNullException("localCacheTimeout");
            if (localCacheFactory == null) throw new ArgumentNullException("localCacheFactory");
            if (backingCacheFactory == null) throw new ArgumentNullException("backingCacheFactory");
            _localCacheTimeout = localCacheTimeout;
            _localCacheFactory = localCacheFactory;
            _backingCacheFactory = backingCacheFactory;
            var section = (NameValueCollection) ConfigurationManager.GetSection(CacheParametersSectionName);
            if (section != null)
                LoadFromConfig(section);
        }


        /// <summary>
        ///   Initializes a new instance of the <see cref="CompositeCacheFactory" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="localCacheTimeout"></param>
        /// <param name="localCacheFactory"></param>
        /// <param name="backingCacheFactory"></param>
        /// <exception cref="System.ArgumentNullException">settings</exception>
        public CompositeCacheFactory([NotNull] NameValueCollection settings,
            [NotNull] ILocalCacheTimeoutStrategy localCacheTimeout,
            [NotNull] ICacheFactory localCacheFactory,
            [NotNull] ICacheFactory backingCacheFactory)
            : base(settings)
        {
            if (localCacheTimeout == null) throw new ArgumentNullException("localCacheTimeout");
            if (localCacheFactory == null) throw new ArgumentNullException("localCacheFactory");
            if (backingCacheFactory == null) throw new ArgumentNullException("backingCacheFactory");

            _localCacheTimeout = localCacheTimeout;
            _localCacheFactory = localCacheFactory;
            _backingCacheFactory = backingCacheFactory;

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            LoadFromConfig(settings);
        }

        /// <summary>
        /// Creates named cache cache.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns></returns>
        protected internal override ICache CreateCache(string instance)
        {
            var cache = base.CreateCache(instance);
            if (cache != null)
                return cache;

            instance = string.IsNullOrEmpty(instance) ? _defaultCacheName : instance;

            Log.InfoFormat("Local cache timeout strategy: {0}", _localCacheTimeout.Describe());

            var localCache = _localCacheFactory.GetCache(instance);
            var remoteCache = _backingCacheFactory.GetCache(instance);

            return new CompositeCache(
                localCache, remoteCache,
                _localCacheTimeout, _devMode);
        }


        /// <summary>
        /// Initializes cache factory.
        /// </summary>
        public override void Initialize()
        {
            if (!IsEnabled) return;

            _backingCacheFactory.Initialize();
            _localCacheFactory.Initialize();

            Log.Info("Initialized");
        }


        void LoadFromConfig(NameValueCollection settings)
        {
            _defaultCacheName = settings["cacheName"];
            bool.TryParse(settings["devMode"], out _devMode);
        }
    }
}