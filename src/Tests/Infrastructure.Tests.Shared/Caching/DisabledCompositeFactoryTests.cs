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
    using NUnit.Framework;


    [TestFixture]
    class DisabledCompositeFactoryTests
    {
        #region Setup/Teardown

        [SetUp]
        public void SetUp()
        {
            var parameters = new NameValueCollection {{"enabled", "false"}};
            _factory = new CompositeCacheFactory(parameters, new FixedTimeoutStrategy(5.Seconds()),
                new NullCacheFactory(), new NullCacheFactory());
            _factory.Initialize();
        }

        #endregion

        CompositeCacheFactory _factory;


        [Test]
        public void CreateCache_Should_CreateNullCache()
        {
            _factory.GetCache()
                .Should().BeOfType<NullCache>();
        }
    }
}