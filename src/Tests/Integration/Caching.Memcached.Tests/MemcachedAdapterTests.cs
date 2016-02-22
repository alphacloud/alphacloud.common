#region copyright

// Copyright 2013-2016 Alphacloud.Net
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
namespace Caching.Memcached.Tests
{
    using System;
    using System.Collections.Generic;
    using Alphacloud.Common.Caching.Memcached;
    using Alphacloud.Common.Infrastructure.Caching;
    using Alphacloud.Common.Testing.Nunit;
    using Enyim.Caching;
    using Enyim.Caching.Memcached;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    class MemcachedAdapterTests
    {
        MemcachedAdapter _adapter;

        Mock<IMemcachedClient> _clientMock;
        Mock<ICacheHealthcheckMonitor> _healthCheck;
        MockRepository _mockery;


        [Test]
        public void Clear_ClearCache()
        {
            _adapter.Clear();

            _clientMock.Verify(c => c.FlushAll());
        }


        [Test]
        public void Get_Should_TransformKey()
        {
            _clientMock.Setup(c => c.Get("test.key"))
                .Returns("value");

            _adapter.Get("key")
                .Should().Be("value");
        }


        [Test]
        public void MultiGet_Should_PerformMultiGetWithTransformedKeys()
        {
            var cachedData = new Dictionary<string, object> {
                {"test.key1", "val1"},
                {"test.key2", "val2"}
            };

            var transformedKeys = new[] {"test.key1", "test.key2"};
            _clientMock.Setup(c => c.Get(Argument.IsCollection(transformedKeys)))
                .Returns(cachedData)
                .Verifiable();

            var res = _adapter.Get(new[] {"key1", "key2"});
            res.Should().Contain(new KeyValuePair<string, object>("key1", "val1"))
                .And.Contain(new KeyValuePair<string, object>("key2", "val2"));
        }


        [Test]
        public void Put_Should_TransformKey()
        {
            _clientMock.Setup(
                cli => cli.Store(StoreMode.Set, It.IsAny<string>(), It.IsAny<object>(), It.IsAny<TimeSpan>()))
                .Returns(true);
            _adapter.Put("key-d", "value", 3.Seconds());

            _clientMock.Verify(c => c.Store(StoreMode.Set, "test.key-d", "value", 3.Seconds()));
        }

        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _mockery = new MockRepository(MockBehavior.Default);
            _clientMock = _mockery.Create<IMemcachedClient>();
            _healthCheck = _mockery.Create<ICacheHealthcheckMonitor>();
            _healthCheck.SetupGet(hc => hc.IsCacheAvailable).Returns(true);
            _adapter = new MemcachedAdapter(_clientMock.Object, _healthCheck.Object, "test");
        }


        [TearDown]
        public void TearDown()
        {
            _adapter = null;
            _mockery.Verify();
        }

        #endregion
    }
}