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
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using System.Threading.Tasks;
    using Alphacloud.Common.Core.Data;
    using Alphacloud.Common.Core.Utils;
    using FluentAssertions;
    using NUnit.Framework;
    using StackExchange.Redis;

    [TestFixture]
    [Category("Integration")]
    class RedisIntegrationTests
    {
        IDatabase _db;
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


        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _db = _redis.GetDatabase();
        }


        [TearDown]
        public void TearDown()
        {
            _db = null;
        }


        [TestFixtureSetUp]
        public void FixtureSetUp()
        {
            var configurationOptions = new ConfigurationOptions {
                ClientName = "Alphacloud.Redis.Tests",
                Password = "Redis--PASS$MSKDF92837bdsh--$dd",
                EndPoints = {"localhost"}
            };

            _redis = ConnectionMultiplexer.Connect(configurationOptions, Console.Out);
        }


        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            Disposer.TryDispose(_redis);
        }

        #endregion

        [Serializable]
        public class CachedItem
        {
            public string Id { get; set; }
            public string Name { get; set; }
        }
    }
}