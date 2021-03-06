﻿#region copyright

// Copyright 2013-2015 Alphacloud.Net
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

namespace Alphacloud.Common.Caching.Redis
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using Core.Data;
    using Core.Utils;
    using Infrastructure.Caching;
    using JetBrains.Annotations;
    using StackExchange.Redis;

    public class RedisConfiguration
    {
        public RedisConfiguration(string instanceName, [NotNull] ConfigurationOptions options, int databaseId = 0)
        {
            if (options == null) throw new ArgumentNullException("options");
            InstanceName = instanceName;
            Options = options;
            DatabaseId = databaseId;
        }


        public RedisConfiguration(string instanceName, [NotNull] ConnectionMultiplexer connection, int databaseId)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            InstanceName = instanceName;
            Connection = connection;
            DatabaseId = databaseId;
        }


        public string InstanceName { get; private set; }
        public ConnectionMultiplexer Connection { get; private set; }
        public int DatabaseId { get; private set; }
        public ConfigurationOptions Options { get; private set; }
    }


    [PublicAPI]
    public class RedisFactory : CacheFactoryBase
    {
        IDictionary<string, RedisConfiguration> _configurationByInstance;
        // todo: customize pool to reject object based on memory size
        IObjectPool<ISerializer> _serializers;


        public RedisFactory([NotNull] IEnumerable<RedisConfiguration> configurationOptions)
        {
            InitInstanceConfiguration(configurationOptions);
        }


        public RedisFactory([NotNull] IEnumerable<RedisConfiguration> configurationOptions,
            [NotNull] NameValueCollection settings) : base(settings)
        {
            InitInstanceConfiguration(configurationOptions);
        }


        void InitInstanceConfiguration([NotNull] IEnumerable<RedisConfiguration> configurationOptions)
        {
            if (configurationOptions == null) throw new ArgumentNullException("configurationOptions");

            _configurationByInstance = configurationOptions.ToDictionary(k => k.InstanceName ?? DefaultInstanceName);
            _serializers = new SerializerPool(16, () => new CompactBinarySerializer(), 256 * 1024);

        }


        public override void Initialize()
        {
            Log.Info("Initialized");
        }


        protected override ICache CreateCache(string instance)
        {
            var cache = base.CreateCache(instance);
            if (cache != null) return cache;

            RedisConfiguration config;
            if (!_configurationByInstance.TryGetValue(instance ?? DefaultInstanceName, out config))
            {
                throw new CacheConfigurationException("Configuration for named cache '{0}' not found");
            }

            var connection = config.Connection ?? ConnectionMultiplexer.Connect(config.Options);
            return new RedisAdapter(NullCacheHealthcheckMonitor.Instance, instance,
                connection.GetDatabase(config.DatabaseId), _serializers);
        }
    }
}