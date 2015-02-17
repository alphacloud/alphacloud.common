#region copyright

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
    using Core.Data;
    using Core.Utils;
    using Infrastructure.Caching;
    using JetBrains.Annotations;
    using StackExchange.Redis;

    class RedisAdapter : CacheBase
    {
        readonly ConnectionMultiplexer _connection;
        readonly IDatabase _db;
        readonly IObjectPool<CompactBinarySerializer> _serializers;


        public RedisAdapter([NotNull] ICacheHealthcheckMonitor monitor, string instanceName,
            [NotNull] ConnectionMultiplexer connection, int databaseId,
            [NotNull] IObjectPool<CompactBinarySerializer> serializers) : base(monitor, instanceName)
        {
            if (connection == null) throw new ArgumentNullException("connection");
            if (serializers == null) throw new ArgumentNullException("serializers");

            _connection = connection;
            _serializers = serializers;
            _db = _connection.GetDatabase(databaseId);
        }


        protected override CacheStatistics DoGetStatistics()
        {
            return new CacheStatistics(false);
        }


        protected override object DoGet(string key)
        {
            var val = _db.StringGet(key);
            if (val.IsNull)
                return null;
            var serializer = _serializers.GetObject();
            try
            {
                return serializer.Deserialize(val);
            }
            finally
            {
                _serializers.ReturnObject(serializer);
            }
        }


        protected override IDictionary<string, object> DoMultiGet(ICollection<string> keys)
        {
            var preparedKeys = new RedisKey[keys.Count];
            keys.ForAll((k, i) => preparedKeys[i] = PrepareCacheKey(k));

            var data = _db.StringGet(preparedKeys);
            
            var result = new Dictionary<string, object>();
            var serializer = _serializers.GetObject();
            try
            {
                keys.ForAll((k, i) => {
                    result[k] = SafeGetValue(data[i], k, serializer);
                });
            }
            finally
            {
                _serializers.ReturnObject(serializer);
            }
            return result;
        }


        object SafeGetValue(RedisValue data, string key, CompactBinarySerializer serializer)
        {
            try
            {
                object item = null;
                if (!data.IsNull)
                {
                    item = serializer.Deserialize(data);
                }
                LogGetSucceed(key, item);
            }
            catch (Exception ex)
            {
                LogGetFaiulre(key, ex);
            }
            return null;
        }


        protected override void DoRemove(string key)
        {
            _db.KeyDelete(key, CommandFlags.FireAndForget);
        }


        protected override void DoPut(string key, object value, TimeSpan ttl)
        {
            var serializer = _serializers.GetObject();
            try
            {
                var buff = serializer.Serialize(value);
                _db.StringSet(key, buff, ttl, flags: CommandFlags.FireAndForget);
            }
            finally
            {
                _serializers.ReturnObject(serializer);
            }
        }


        protected override void DoClear()
        {
            Log.Warn("Clear not implemented"); // todo: implement
        }
    }
}