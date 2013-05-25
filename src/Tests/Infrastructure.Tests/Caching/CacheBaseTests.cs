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

namespace Infrastructure.Tests.Caching
{
    using System;

    using Alphacloud.Common.Infrastructure.Caching;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    using Testing.Nunit;

    //// ReSharper disable InconsistentNaming


    [TestFixture]
    class CacheBaseTests : MockedTestsBase
    {
        protected override void DoSetup()
        {
            _healthcheckMock = Mockery.Create<ICacheHealthcheckMonitor>();
            _cacheMock = new Mock<CacheBase>(_healthcheckMock.Object, CacheName) { CallBase = true };
            _cache = _cacheMock.Object;
        }


        protected override void DoTearDown()
        {
            _cache = null;
        }


        const string Key = "key";
        const string CacheName = "BaseCache";
        const string CacheKey = CacheName + "." + Key;

        Mock<ICacheHealthcheckMonitor> _healthcheckMock;
        Mock<CacheBase> _cacheMock;
        CacheBase _cache;


        void SetCacheUnavailable()
        {
            _healthcheckMock.SetupGet(m => m.IsCacheAvailable).Returns(false);
        }


        void SetCacheAvailable()
        {
            _healthcheckMock.SetupGet(m => m.IsCacheAvailable).Returns(true);
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
        public void Put_CacheIsAvailable_Should_PutItemToCache()
        {
            SetCacheAvailable();

            _cache.Put(Key, "1", 1.Seconds());
            _cacheMock.Verify(c => c.DoPut(CacheKey, "1", 1.Seconds()));
        }


        [Test]
        public void Put_CacheIsNotAvailable_Should_SkipPut()
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
                .Throws<InvalidOperationException>()
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
        public void Remove_CacheIsNotAvailable_Should_SkipRemove()
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
                .Throws<InvalidOperationException>()
                .Verifiable();

            _cache.Remove(Key);
            _cacheMock.Verify();
        }
    }

    //// ReSharper restore InconsistentNaming
}
