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

// ReSharper disable ExceptionNotDocumented
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
namespace Caching.Redis.Tests
{
    using System.Collections.Generic;
    using System.Text;
    using Alphacloud.Common.Caching.Redis;
    using Alphacloud.Common.Core.Data;
    using Alphacloud.Common.Core.Utils;
    using Alphacloud.Common.Infrastructure.Caching;
    using Alphacloud.Common.Testing.Nunit;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;
    using StackExchange.Redis;

    [TestFixture]
    class RedisAdapterTests : MockedTestsBase
    {
        RedisAdapter _adapter;
        Mock<IDatabase> _databaseMock;
        Mock<ICacheHealthcheckMonitor> _healthcheckMock;
        ISerializer _serializer;


        protected override void DoSetup()
        {
            _healthcheckMock = Mockery.Create<ICacheHealthcheckMonitor>();
            _healthcheckMock.SetupGet(hc => hc.IsCacheAvailable).Returns(true);

            _databaseMock = Mockery.Create<IDatabase>();
            _databaseMock.As<IDatabaseAsync>();

            var serializers = new ObjectPool<ISerializer>(10, () => new StringSerializer());
            _serializer = serializers.GetObject();

            _adapter = new RedisAdapter(_healthcheckMock.Object, "redisTest", _databaseMock.Object, serializers);
        }


        RedisValue Serialized(object val)
        {
            return _serializer.Serialize(val);
        }


        [Test]
        public void Get_Should_TransformKey()
        {
            _databaseMock.Setup(c => c.StringGet("redisTest.key", CommandFlags.None))
                .Returns(Serialized("value"));

            _adapter.Get("key")
                .Should().Be("value", "object was not transformed before requesting from cache");
        }


        [Test]
        public void MultiPut_Should_PerformPutWithTransformedKeys()
        {
            var data = new[] {
                new KeyValuePair<string, object>("key1", "value1"),
                new KeyValuePair<string, object>("key2", "value2")
            };

            _adapter.Put(data, 3.Seconds());

            _databaseMock.Verify(db => db.StringSet("redisTest.key1", Serialized("value1"), 3.Seconds(), When.Always,
                CommandFlags.FireAndForget));

            _databaseMock.Verify(db => db.StringSet("redisTest.key2", Serialized("value2"), 3.Seconds(), When.Always,
                CommandFlags.FireAndForget));
        }


        [Test]
        public void MultiGet_Should_PerformMultiGetWithTransformedKeys()
        {
            var cachedData = new[] {
                Serialized("val1"),
                Serialized("val2")
            };

            var transformedKeys = new RedisKey[] {"redisTest.key1", "redisTest.key2"};
            _databaseMock.Setup(c => c.StringGet(Argument.IsArray(transformedKeys), CommandFlags.None))
                .Returns(cachedData)
                .Verifiable();

            var res = _adapter.Get(new[] {"key1", "key2"});
            res.Should().Contain(new KeyValuePair<string, object>("key1", "val1"))
                .And.Contain(new KeyValuePair<string, object>("key2", "val2"));
        }


        [Test]
        public void MultiGet_Should_ReturnNullForMissingItems()
        {
            var cachedData = new[] {
                RedisValue.Null,
                Serialized("val2")
            };

            _databaseMock.Setup(c => c.StringGet(It.IsAny<RedisKey[]>(), CommandFlags.None))
                .Returns(cachedData)
                .Verifiable();

            var res = _adapter.Get(new[] {"key1", "key2"});
            res.Should()
                .Contain(new KeyValuePair<string, object>("key1", null))
                .And.Contain(new KeyValuePair<string, object>("key2", "val2"));
        }


        [Test]
        public void Put_Should_TransformKey()
        {
            _adapter.Put("key-d", "value", 3.Seconds());

            _databaseMock.Verify(db =>
                db.StringSet("redisTest.key-d", It.Is<RedisValue>(v => Encoding.UTF8.GetString(v) == "value"),
                    3.Seconds(), When.Always, CommandFlags.FireAndForget));
        }


        [Test]
        public void Remove_Should_DeleteItemFromCache()
        {
            _adapter.Remove("key1");

            _databaseMock.Verify(db => db.KeyDelete("redisTest.key1", CommandFlags.FireAndForget), "key was not removed");
        }
    }
}