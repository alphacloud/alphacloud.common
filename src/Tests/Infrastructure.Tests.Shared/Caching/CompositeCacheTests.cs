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

// ReSharper disable ExceptionNotDocumented
// ReSharper disable HeapView.ClosureAllocation
// ReSharper disable ExceptionNotDocumentedOptional
// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable HeapView.ObjectAllocation
// ReSharper disable HeapView.ObjectAllocation.Evident
namespace Infrastructure.Tests.Caching
{
    using System.Collections.Generic;
    using System.Linq;
    using Alphacloud.Common.Infrastructure.Caching;
    using Alphacloud.Common.Testing.Nunit;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    //// ReSharper disable InconsistentNaming

    [TestFixture]
    class CompositeCacheTests : CompositeCacheTestsBase
    {
        const string Key = "key";
        const string Value = "val";
        CompositeCache _cache;


        protected override void DoSetup()
        {
            base.DoSetup();
            _cache = new CompositeCache(LocalCache.Object, RemoteCache.Object, new FixedTimeoutStrategy(2.Seconds()));
        }


        [Test]
        public void Clear_Should_ClearCaches()
        {
            _cache.Clear();

            LocalCache.Verify(localCache => localCache.Clear());
            RemoteCache.Verify(remoteCache => remoteCache.Clear());
        }


        [Test]
        public void GetStatistics_Should_CollectStatistics()
        {
            var localCacheStats = new CacheStatistics(10, 100, 100, 50);
            var remoteCacheStats = new CacheStatistics(5, 20, 100, 100);

            LocalCache.Setup(c => c.GetStatistics()).Returns(localCacheStats);
            RemoteCache.Setup(c => c.GetStatistics()).Returns(remoteCacheStats);

            var stats = _cache.GetStatistics();

            stats.IsSuccess.Should().BeTrue();
            stats.Nodes.Should().HaveCount(1);

            stats.GetCount.Should().Be(120);
            stats.HitCount.Should().Be(15);
            stats.PutCount.Should().Be(100);
            stats.ItemCount.Should().Be(150);
            stats.HitRate.Should().Be(12);

            var localStats = stats.Nodes.First();
            localStats.Server.Should().Be("LocalCache");
            localStats.GetCount.Should().Be(100);
            localStats.HitCount.Should().Be(10);
            localStats.PutCount.Should().Be(100);
            localStats.ItemCount.Should().Be(50);
            localStats.HitRate.Should().Be(10);
        }


        [Test]
        public void Get_FromRemoteCache_Should_UpdateLocal()
        {
            RemoteCache.Setup(c => c.Get(Key))
                .Returns(Value);

            _cache.Get(Key).Should().Be(Value, "data lost");

            LocalCache.Verify(c => c.Put(Key, Value, 2.Seconds()), "should update local cache");
        }


        [Test]
        public void Get_Should_ReturnDataFromLocaCache()
        {
            LocalCache.Setup(c => c.Get(Key))
                .Returns(Value);

            _cache.Get(Key)
                .Should().Be(Value, "wrong data returned from cache");
            RemoteCache.Verify(c => c.Get(Key), Times.Never(),
                "should not access remote cache if local cache contains data");
        }


        [Test]
        public void MultiGet_Should_TryLocalCacheFirst()
        {
            var localCacheResponse = new Dictionary<string, object> {
                {"k1", "v1"},
                {"k2", "v2"},
                {"k3", null}
            };
            var remoteCacheResponse = new Dictionary<string, object> {
                {"k3", "v3"}
            };

            LocalCache.Setup(lc => lc.Get(Argument.IsCollection(new[] {"k1", "k2"})))
                .Returns(localCacheResponse);
            RemoteCache.Setup(rc => rc.Get(new[] {"k3"}))
                .Returns(remoteCacheResponse);

            var res = _cache.Get(new[] {"k1", "k2", "k3"});
            res.Should().Contain(new KeyValuePair<string, object>("k1", "v1"), "local cache failed");
            res.Should().Contain(new KeyValuePair<string, object>("k2", "v2"), "local cache failed");
            res.Should().Contain(new KeyValuePair<string, object>("k3", "v3"), "remote cache failed");
        }


        [Test]
        public void MultiGet_Should_UpdateLocalCacheWithFreshData()
        {
            var localResponse = new Dictionary<string, object> {
                {"k1", null},
                {"k2", null}
            };
            var remoteResponse = new Dictionary<string, object> {
                {"k1", "v1"},
                {"k2", null}
            };

            var keys = new[] {"k1", "k2"};
            LocalCache.Setup(lc => lc.Get(Argument.IsCollection(keys)))
                .Returns(localResponse);
            RemoteCache.Setup(rc => rc.Get(Argument.IsCollection(keys)))
                .Returns(remoteResponse);

            var res = _cache.Get(keys);
            res.Should().Contain(new KeyValuePair<string, object>("k1", "v1"))
                .And.Contain(new KeyValuePair<string, object>("k2", null),
                    "remote cache results not returned");

            LocalCache.Verify(lc => lc.Put("k1", "v1", 2.Seconds()), "Local Cache should be updated with new data");
            LocalCache.Verify(lc => lc.Remove("k2"), "obsolete data should be removed from cache");
        }


        [Test]
        public void MultiPut_Should_UpdateLocalCache()
        {
            var data = new Dictionary<string, object> {
                {"k1", "v1"},
                {"k2", "v2"}
            };

            _cache.Put(data, 1.Minutes());

            LocalCache.Verify(lc => lc.Put(Argument.IsDictionary(data), 2.Seconds()), "local cache not updated");
        }


        [Test]
        public void MultiPut_Should_UpdateRemoteCache()
        {
            var data = new Dictionary<string, object> {
                {"k1", "v1"},
                {"k2", "v2"}
            };

            _cache.Put(data, 1.Minutes());

            LocalCache.Verify(lc => lc.Put(Argument.IsDictionary(data), 2.Seconds()), "local cache not updated");
        }


        [Test]
        public void Put_Should_UpdateCaches()
        {
            _cache.Put(Key, Value, 30.Seconds());

            LocalCache.Verify(c => c.Put(Key, Value, 2.Seconds()), "should update local cache with LocalTimeout");
            RemoteCache.Verify(c => c.Put(Key, Value, 30.Seconds()), "should update remote cache");
        }


        [Test]
        public void Remove_Should_RemoveDataFromCaches()
        {
            _cache.Remove(Key);

            LocalCache.Verify(c => c.Remove(Key), "should remove from local cache");
            RemoteCache.Verify(c => c.Remove(Key), "should remove from remote cache");
        }
    }

    //// ReSharper restore InconsistentNaming
}