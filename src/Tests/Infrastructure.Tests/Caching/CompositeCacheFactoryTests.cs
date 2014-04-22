#region copyright

// Copyright 2013-2014 Alphacloud.Net
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
    using System.Collections.Specialized;
    using Alphacloud.Common.Infrastructure.Caching;
    using FluentAssertions;
    using global::Castle.MicroKernel.Registration;
    using global::Castle.Windsor;
    using Moq;
    using NUnit.Framework;

    //// ReSharper disable InconsistentNaming

    #region using

    #endregion

    [TestFixture]
    class CompositeCacheFactoryTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            _kernel = new WindsorContainer();
            _mockery = new MockRepository(MockBehavior.Default);

            _localCache = _mockery.Create<ICache>();
            _localCacheFactory = _mockery.Create<ICacheFactory>();
            _localCacheFactory.Setup(f => f.GetCache(It.IsAny<string>()))
                .Returns(_localCache.Object);
            _kernel.Register(
                Component.For<ICacheFactory>()
                    .Instance(new CacheFactoryWrapper(_localCacheFactory.Object))
                    .Named(CompositeCache.LocalCacheInstanceName));

            _remoteCache = _mockery.Create<ICache>();
            _remoteCacheFactory = _mockery.Create<ICacheFactory>();
            _remoteCacheFactory.Setup(f => f.GetCache(It.IsAny<string>()))
                .Returns(_remoteCache.Object);

            _timeoutStrategy = new FixedTimeoutStrategy(5.Seconds());

            _kernel.Register(
                Component.For<ICacheFactory>()
                    .Instance(new CacheFactoryWrapper(_remoteCacheFactory.Object))
                    .Named(CompositeCache.BackingCacheInstanceName));
        }


        [TearDown]
        public void TearDown()
        {
            _mockery.Verify();
        }

        #endregion

        class CacheFactoryWrapper : ICacheFactory
        {
            readonly ICacheFactory _innerFactory;


            public CacheFactoryWrapper(ICacheFactory innerFactory)
            {
                _innerFactory = innerFactory;
            }

            #region ICacheFactory Members

            public ICache GetCache(string instance = null)
            {
                return _innerFactory.GetCache(instance);
            }


            public void Initialize()
            {
                _innerFactory.Initialize();
            }

            #endregion
        }


        MockRepository _mockery;
        Mock<ICache> _localCache;
        Mock<ICache> _remoteCache;
        WindsorContainer _kernel;
        Mock<ICacheFactory> _localCacheFactory;
        Mock<ICacheFactory> _remoteCacheFactory;
        FixedTimeoutStrategy _timeoutStrategy;


        [Test]
        public void EnsureCacheFactoiesResolved()
        {}


        [Test]
        public void GetCache_DefaultName_Should_CreateCacheWithDefaultNames()
        {
            var settings = new NameValueCollection();
            settings["cacheName"] = "cache1";
            var factory = new CompositeCacheFactory(settings, _timeoutStrategy, _localCacheFactory.Object,
                _remoteCacheFactory.Object);
            factory.Initialize();
            factory.GetCache();

            _localCacheFactory.Verify(localFactory => localFactory.GetCache("cache1"));
            _remoteCacheFactory.Verify(remoteFactory => remoteFactory.GetCache("cache1"));
        }


        [Test]
        public void GetCache_WithNameSpecified_Should_CreateCacheWithCustomName()
        {
            var factory = new CompositeCacheFactory(new NameValueCollection(), _timeoutStrategy,
                _localCacheFactory.Object, _remoteCacheFactory.Object);
            factory.Initialize();
            factory.GetCache("customCache");

            _localCacheFactory.Verify(local => local.GetCache("customCache"));
            _remoteCacheFactory.Verify(remote => remote.GetCache("customCache"));
        }


        [Test]
        public void ShouldResolveNamedInstances()
        {
            var local = _kernel.Resolve<ICacheFactory>(CompositeCache.LocalCacheInstanceName);
            var remote = _kernel.Resolve<ICacheFactory>(CompositeCache.BackingCacheInstanceName);

            local.Should().NotBeSameAs(remote);
        }


        [Test]
        public void Should_ReadParameters_FromAppConfig()
        {
            var factory = new CompositeCacheFactory(_timeoutStrategy, _localCacheFactory.Object,
                _remoteCacheFactory.Object);
            factory.Initialize();
            factory.IsEnabled.Should().BeTrue();
        }
    }


    //// ReSharper restore InconsistentNaming
}