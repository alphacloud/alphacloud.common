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
    using System.Collections.Specialized;

    using Alphacloud.Common.Infrastructure.Caching;
    using Alphacloud.Common.ServiceLocator.Castle;

    using Castle.MicroKernel.Registration;
    using Castle.Windsor;

    using FluentAssertions;

    using Microsoft.Practices.ServiceLocation;

    using Moq;

    using NUnit.Framework;

    //// ReSharper disable InconsistentNaming

    #region using

    #endregion

    [TestFixture]
    class CompositeCacheFactoryTests
    {
        [SetUp]
        public void SetUp()
        {
            _kernel = new WindsorContainer();
            var locator = new WindsorServiceLocatorAdapter(_kernel);
            ServiceLocator.SetLocatorProvider(() => locator);

            _mockery = new MockRepository(MockBehavior.Default);

            _localCache = _mockery.Create<ICache>();
            _localCacheFactory = _mockery.Create<ICacheFactory>();
            _localCacheFactory.Setup(f => f.GetCache(It.IsAny<string>()))
                .Returns(_localCache.Object);
            _kernel.Register(Component.For<ICacheFactory>().Instance(_localCacheFactory.Object).Named("local"));


            _remoteCache = _mockery.Create<ICache>();
            _remoteCacheFactory = _mockery.Create<ICacheFactory>();
            _remoteCacheFactory.Setup(f => f.GetCache(It.IsAny<string>()))
                .Returns(_remoteCache.Object);
            _kernel.Register(Component.For<ICacheFactory>().Instance(_remoteCacheFactory.Object).Named("remote"));
        }


        [TearDown]
        public void TearDown()
        {
            _mockery.Verify();
        }


        MockRepository _mockery;
        Mock<ICache> _localCache;
        Mock<ICache> _remoteCache;
        WindsorContainer _kernel;
        Mock<ICacheFactory> _localCacheFactory;
        Mock<ICacheFactory> _remoteCacheFactory;


        [Test]
        public void GetCache_DefaultName_Should_CreateCacheWithDefaultNames()
        {
            var settings = new NameValueCollection();
            settings["cacheName"] = "cache1";
            var factory = new CompositeCacheFactory(settings);
            factory.Initialize();
            factory.GetCache();

            _localCacheFactory.Verify(f => f.GetCache("cache1"));
            _remoteCacheFactory.Verify(f => f.GetCache("cache1"));
        }


        [Test]
        public void GetCache_WithNameSpecified_Should_CreateCacheWithCustomName()
        {
            var factory = new CompositeCacheFactory(new NameValueCollection());
            factory.Initialize();
            factory.GetCache("customCache");

            _localCacheFactory.Verify(f => f.GetCache("customCache"));
            _remoteCacheFactory.Verify(f => f.GetCache("customCache"));
        }


        [Test]
        public void Should_ReadParameters_FromAppConfig()
        {
            var factory = new CompositeCacheFactory();
            factory.Initialize();
            factory.IsEnabled.Should().BeTrue();
        }
    }

    //// ReSharper restore InconsistentNaming
}
