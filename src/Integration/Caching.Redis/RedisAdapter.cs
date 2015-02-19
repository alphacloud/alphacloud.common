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
        readonly IDatabase _db;
        readonly IObjectPool<IBinarySerializer> _serializers;


        /// <summary>
        ///   Initializes a new instance of the <see cref="RedisAdapter" /> class.
        /// </summary>
        /// <param name="monitor">The healthcheck monitor.</param>
        /// <param name="instanceName">Name of the instance.</param>
        /// <param name="redisDatabase">The redis database reference.</param>
        /// <param name="serializers">The serializers pool.</param>
        /// <exception cref="System.ArgumentNullException">
        ///   redisDatabase or serializers is <c>null</c>.
        /// </exception>
        public RedisAdapter(
            [NotNull] ICacheHealthcheckMonitor monitor,
            string instanceName, [NotNull] IDatabase redisDatabase,
            [NotNull] IObjectPool<IBinarySerializer> serializers) : base(monitor, instanceName)
        {
            if (redisDatabase == null) throw new ArgumentNullException("redisDatabase");
            if (serializers == null) throw new ArgumentNullException("serializers");

            _serializers = serializers;
            _db = redisDatabase;
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

            IDictionary<string, object> result;
            var serializer = _serializers.GetObject();
            try
            {
                result = BuildResultDictionary(keys, data, (k, v) => SafeGetValue(v, k, serializer));
            }
            finally
            {
                _serializers.ReturnObject(serializer);
            }
            return result;
        }


        static IDictionary<string, object> BuildResultDictionary<TCachedValue>(ICollection<string> keys,
            IList<TCachedValue> cachedValues,
            Func<string, TCachedValue, object> retrieveValue)
        {
            if (keys.Count != cachedValues.Count)
            {
                throw new InvalidOperationException(
                    "Result values count does not match key count, keys: {0}, values: {1}".ApplyArgs(keys.Count,
                        cachedValues.Count));
            }

            var res = new SortedList<string, object>(keys.Count);
            keys.ForAll((k, i) => { res[k] = retrieveValue(k, cachedValues[i]); });

            return res;
        }


        object SafeGetValue(RedisValue data, string key, IBinarySerializer serializer)
        {
            try
            {
                object item = null;
                if (!data.IsNull)
                {
                    return serializer.Deserialize(data);
                }
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