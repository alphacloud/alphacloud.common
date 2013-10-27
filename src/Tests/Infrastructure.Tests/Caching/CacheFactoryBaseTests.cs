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
    using Alphacloud.Common.Testing.Nunit;
    using FluentAssertions;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    class CacheFactoryBaseTests : MockedTestsBase
    {
        Mock<CacheFactoryBase> _factoryMock;
        Mock<ICache> _cacheMock;


        protected override void DoSetup()
        {
            _cacheMock = Mockery.Create<ICache>();

            _factoryMock = Mockery.Create<CacheFactoryBase>(MockBehavior.Loose);
        }


        [Test]
        public void Dispose_Should_DisposeCaches()
        {
            _factoryMock.CallBase = true;

            var disposableCacheMock = _cacheMock.As<IDisposable>();
            _factoryMock.Setup(factory => factory.CreateCache(It.IsAny<string>())).Returns(_cacheMock.Object);

            _factoryMock.Object.GetCache().Should().Be(_cacheMock.Object);
            _factoryMock.Object.Dispose();
            disposableCacheMock.Verify(cache => cache.Dispose(), Times.AtLeastOnce());
        }
    }
}