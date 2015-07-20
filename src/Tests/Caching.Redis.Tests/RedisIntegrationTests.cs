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

namespace Caching.Redis.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Alphacloud.Common.Caching.Redis;
    using Alphacloud.Common.Core.Data;
    using Alphacloud.Common.Core.Utils;
    using Alphacloud.Common.Infrastructure.Caching;
    using FluentAssertions;
    using JetBrains.Annotations;
    using NUnit.Framework;
    using StackExchange.Redis;

    [TestFixture]
    [Category("Integration")]
    class RedisIntegrationTests
    {
        const string CacheInstance = "Alphacloud.Caching.Redis.Tests";
        ICache _cache;
        IDatabase _db;
        RedisFactory _factory;
        string _g;
        string _key1;
        string _key2;
        ConnectionMultiplexer _redis;


        [Test]
        public async Task CanStoreString()
        {
            var res = await _db.StringSetAsync("key1", "value1", TimeSpan.FromSeconds(10))
                .ContinueWith(c => _db.StringGet("key1"), TaskContinuationOptions.OnlyOnRanToCompletion);
            res.ToString().Should().Be("value1");
        }


        [Test]
        public void CanStoreObject()
        {
            var obj = new CachedItem {
                Id = "id",
                Name = "name"
            };

            var formatter = new CompactBinarySerializer();

            var key = Guid.NewGuid().ToString();
            var serializedValue = formatter.Serialize(obj);
            _db.StringSet(key, serializedValue, TimeSpan.FromSeconds(60));

            var val = _db.StringGet(key);
            val.IsNull.Should().BeFalse();
            var newValue = (byte[]) val;
            newValue.Should().Equal(serializedValue);
        }


        [Test]
        public async void CanMultiGet()
        {
            _db.StringSet(_key2, "value", 10.Seconds());
            var res = await _db.StringGetAsync(new RedisKey[] {_key1, _key2});

            res[0].IsNull.Should().BeTrue();
            res[1].ToString().Should().Be("value");
        }


        [Test]
        public void Put_Should_AddItemToCache()
        {
            var key = Guid.NewGuid().ToString("D");
            var value = "val." + key;
            _cache.Put(key, value, 3.Seconds());
            _cache.Get<string>(key).Should().Be(value, "failed to retrieve value");
        }


        [Test]
        public async void Put_Should_AddItemToCacheWithExpiration()
        {
            var key = Guid.NewGuid().ToString("D");
            var value = "val." + key;
            _cache.Put(key, value, 3.Seconds());

            await Task.Delay(3.Seconds());
            _cache.Get<string>(key).Should().BeNull("expired key received from cache");
        }


        [Test]
        public void MultiPut_Should_StoreAllItems()
        {
            var g = Guid.NewGuid().ToString("N");
            var k1 = "key1-" + g;
            var k2 = "key2-" + g;
            _cache.Put(
                new[] {new KeyValuePair<string, object>(k1, "val1"), new KeyValuePair<string, object>(k2, "val2")},
                3.Seconds());

            _cache.Get<string>(k1).Should().Be("val1");
            _cache.Get<string>(k2).Should().Be("val2");
        }


        [Test]
        public void MultiGet_Should_RetrieveMultipleItems()
        {
            var g = Guid.NewGuid().ToString("N");
            var k1 = "key1-" + g;
            var k2 = "key2-" + g;

            _cache.Put(k1, "val1", 3.Seconds());
            _cache.Put(k2, "val2", 3.Seconds());

            var res = _cache.Get(new[] {k1, k2});

            res.Should().HaveCount(2);
            res[k1].Should().Be("val1");
            res[k2].Should().Be("val2");
        }


        [Test]
        public void CanGetStatistics()
        {
            var stat = _cache.GetStatistics();
        }

        #region Nested type: CachedItem

        [Serializable]
        class CachedItem
        {
            [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
            public string Id { get; set; }

            [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
            public string Name { get; set; }
        }

        #endregion

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _cache = _factory.GetCache(CacheInstance);
            _g = Guid.NewGuid().ToString("D");
            _key1 = "key1-" + _g;
            _key2 = "key2_" + _g;
        }


        [TearDown]
        public void TearDown()
        {
            _cache = null;
        }


        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            var configurationOptions = new ConfigurationOptions {
                ClientName = "Alphacloud.Redis.Tests",
                EndPoints = { "localhost:6379", "localhost:6380" },
                AllowAdmin = true,
            };

            //_streamManager = new RecyclableMemoryStreamManager(32 * 1024, 256 * 1024, 4 * 1024 * 1024);
            _redis = ConnectionMultiplexer.Connect(configurationOptions, Console.Out);
            _db = _redis.GetDatabase();
            _factory = new RedisFactory(new[] {
                new RedisConfiguration(CacheInstance, _redis, 0)
            });
            _factory.Initialize();
        }


        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Disposer.TryDispose(_redis);
        }

        #endregion
    }
}