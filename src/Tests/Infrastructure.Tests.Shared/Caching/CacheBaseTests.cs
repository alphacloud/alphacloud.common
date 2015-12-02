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

namespace Infrastructure.Tests.Caching
{
    using System;
    using System.Collections.Generic;
    using Alphacloud.Common.Infrastructure.Caching;
    using Alphacloud.Common.Testing.Nunit;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    //// ReSharper disable InconsistentNaming


    [TestFixture]
    class CacheBaseTests : MockedTestsBase
    {
        const string Key = "key";
        const string CacheName = "BaseCache";
        const string CacheKey = CacheName + "." + Key;
        CacheBase _cache;
        Mock<CacheBase> _cacheMock;
        Mock<ICacheHealthcheckMonitor> _healthcheckMock;


        protected override void DoSetup()
        {
            _healthcheckMock = Mockery.Create<ICacheHealthcheckMonitor>();
            _cacheMock = new Mock<CacheBase>(_healthcheckMock.Object, CacheName) {CallBase = true};
            _cache = _cacheMock.Object;
        }


        protected override void DoTearDown()
        {
            _cache = null;
        }


        void SetCacheUnavailable()
        {
            _healthcheckMock.SetupGet(m => m.IsCacheAvailable).Returns(false);
        }


        void SetCacheAvailable()
        {
            _healthcheckMock.SetupGet(m => m.IsCacheAvailable).Returns(true);
        }


        [Test]
        public void Clear_CacheIsAvaiable_Should_ClearUnderlyingCache()
        {
            SetCacheAvailable();
            _cache.Clear();

            _cacheMock.Verify(c => c.DoClear(), Times.Once());
        }


        [Test]
        public void Clear_CacheIsUnavailable_ShouldSkipClear()
        {
            SetCacheUnavailable();
            _cache.Clear();

            _cacheMock.Verify(c => c.DoClear(), Times.Never(), "should not access unavailable cache");
        }


        [Test]
        public void GetStatistics_CacheIsAvailable_Should_GetStatisticsFromUnderlyingCache()
        {
            SetCacheAvailable();

            var statistics = new CacheStatistics(10, 20, 30, 25);
            _cacheMock.Setup(c => c.DoGetStatistics()).Returns(statistics)
                .Verifiable();

            _cache.GetStatistics().Should().Be(statistics);
        }


        [Test]
        public void GetStatistics_CacheIsUnavailable_Should_SkipStatisticsRetrieval()
        {
            SetCacheUnavailable();
            _cache.GetStatistics();

            _cacheMock.Verify(c => c.DoGetStatistics(), Times.Never(), "should not access unavailable cache");
        }


        [Test]
        public void Get_CacheIsAvaialble_Should_InvokeUnderlyingCache()
        {
            SetCacheAvailable();
            _cache.Get(Key);
            _cacheMock.Verify(c => c.DoGet(CacheKey));
        }


        [Test]
        public void Get_CacheIsNotAvaialble_Should_ReturnNull()
        {
            SetCacheUnavailable();

            _cache.Get("1").Should().BeNull();
            _cacheMock.Verify(c => c.DoGet(It.IsAny<string>()), Times.Never(),
                "should not access cache if not avaialble");
        }


        [Test]
        public void Get_UnderlyingCacheException_Should_ReturnNull()
        {
            SetCacheAvailable();
            _cacheMock.Setup(c => c.DoGet(CacheKey))
                .Throws(new Exception("Test exception"))
                .Verifiable();

            _cache.Get(Key).Should().BeNull();
            _cacheMock.Verify();
        }


        [Test]
        public void MiltiGet_OnUnderlyingCacheException_Should_UseNullValueAndProcessNextCacheKey()
        {
            SetCacheAvailable();

            _cacheMock.Setup(c => c.DoGet("BaseCache.err"))
                .Throws<Exception>();
            _cacheMock.Setup(c => c.DoGet("BaseCache.key"))
                .Returns("val");

            var res = _cache.Get(new[] {"err", "key"});
            res.Should().ContainKeys("err", "key");

            res["err"].Should().BeNull("exception occured in underlying cache");
            res["key"].Should().Be("val");
        }


        [Test]
        public void MultiGet_CacheIsAvaiable_ShouldReturnDictionaryWithAllKeys()
        {
            SetCacheAvailable();
            _cacheMock.Setup(c => c.DoGet("BaseCache.key1"))
                .Returns(null)
                .Verifiable();
            _cacheMock.Setup(c => c.DoGet("BaseCache.key2"))
                .Returns("val2")
                .Verifiable();

            var res = _cache.Get(new[] {"key1", "key2"});

            res.Should().HaveCount(2, "should return results for all keys");
            res["key1"].Should().BeNull("no data in cache");
            res["key2"].Should().Be("val2");
        }


        [Test]
        public void MultiGet_CacheIsNotAvailable_Should_SkipGet()
        {
            SetCacheUnavailable();

            var res = _cache.Get(new[] {"key1", "key2"});

            res.Should().HaveCount(2);
            res["key1"].Should().BeNull("key1");
            res["key2"].Should().BeNull("key2");

            _cacheMock.Verify(c => c.DoGet(It.IsAny<string>()), Times.Never(), "cache is not enabled");
        }


        [Test]
        public void Put_CacheIsAvailable_Should_PutItemToCache()
        {
            SetCacheAvailable();

            _cache.Put(Key, "1", 1.Seconds());
            _cacheMock.Verify(c => c.DoPut(CacheKey, "1", 1.Seconds()));
        }


        [Test]
        public void Put_CacheIsUnavailable_Should_SkipPut()
        {
            SetCacheUnavailable();

            _cache.Put(Key, "1", 1.Seconds());
            _cacheMock.Verify(c => c.DoPut(It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()),
                Times.Never());
        }


        [Test]
        public void Put_NullItem_Should_RemoveItem()
        {
            SetCacheAvailable();

            _cache.Put(Key, null, 5.Seconds());

            _cacheMock.Verify(c => c.DoRemove(CacheKey), "should remove Null item from cache");
            _cacheMock.Verify(c => c.DoPut(CacheKey, It.IsAny<object>(), It.IsAny<TimeSpan>()), Times.Never(),
                "should not add Null item to cache");
        }


        [Test]
        public void Put_UnderlyingCacheException_Should_LogException()
        {
            SetCacheAvailable();
            _cacheMock.Setup(c => c.DoPut(CacheKey, "2", 1.Seconds()))
                .Throws(new Exception("Test exception"))
                .Verifiable();

            _cache.Put(Key, "2", 1.Seconds());
            _cacheMock.Verify();
        }


        [Test]
        public void Remove_CacheIsAvaiable_Should_RemoveFromCache()
        {
            SetCacheAvailable();

            _cache.Remove(Key);
            _cacheMock.Verify(c => c.DoRemove(CacheKey));
        }


        [Test]
        public void Remove_CacheIsUnavailable_Should_SkipRemove()
        {
            SetCacheUnavailable();

            _cache.Remove(Key);
            _cacheMock.Verify(c => c.DoRemove(Key), Times.Never(), "should not access cache if not avaialble");
        }


        [Test]
        public void Remove_UnderlyingCacheException_Should_LogException()
        {
            SetCacheAvailable();
            _cacheMock.Setup(c => c.DoRemove(CacheKey))
                .Throws(new Exception("Test exception"))
                .Verifiable();

            _cache.Remove(Key);
            _cacheMock.Verify();
        }


        [Test]
        public void DictionaryShouldKeepKeysForNullValues()
        {
            // used in Log MultiGet statistics. Make sure we are returning missing cache values as nulls.
            var d = new Dictionary<string, object> {
                {"1", 1},
                {"null", null}
            };
            d.Keys.Should().Contain("null");
        }
    }

    //// ReSharper restore InconsistentNaming
}