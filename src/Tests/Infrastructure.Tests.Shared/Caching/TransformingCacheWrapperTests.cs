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
    using Alphacloud.Common.Infrastructure.Transformations;

    using FluentAssertions;

    using Moq;

    using NUnit.Framework;

    // ReSharper disable InconsistentNaming


    [TestFixture]
    class TransformingCacheWrapperTests
    {
        [SetUp]
        public void SetUp()
        {
            _cacheMock = new Mock<ICache>();
            _cacheMock.SetupGet(c => c.Name).Returns("mock");
            _cache = new TransformingCacheWrapper(_cacheMock.Object, "wrapper", new KeyTransformer(),
                new ValueTransformer());
        }

        [TearDown]
        public void TearDown()
        {
        }

        class KeyTransformer : IEncoder<string>
        {
            public string Encode(string source)
            {
                return source + ".key";
            }
        }


        class ValueTransformer : ITransformer<object>
        {
            public const string EncodedSuffix = "(encoded)";

            public object Encode(object source)
            {
                return source + EncodedSuffix;
            }

            public object Decode(object source)
            {
                var str = source.ToString();
                return str.Substring(0, str.Length - EncodedSuffix.Length);
            }
        }

        Mock<ICache> _cacheMock;
        TransformingCacheWrapper _cache;

        [Test]
        public void Get_Should_DecodeValue()
        {
            _cacheMock.Setup(c => c.Get("11.key"))
                .Returns("value" + ValueTransformer.EncodedSuffix);

            _cache.Get<string>("11")
                .Should().Be("value");
        }

        [Test]
        public void Get_Should_EncodeKey()
        {
            _cacheMock.Setup(c => c.Get(It.IsAny<string>())).Returns("value" + ValueTransformer.EncodedSuffix);
            _cache.Get("1");
            _cacheMock.Verify(c => c.Get("1.key"));
        }

        [Test]
        public void Put_Should_EncodeKey()
        {
            var ttl = TimeSpan.FromSeconds(10);
            _cache.Put("1", new object(), ttl);

            _cacheMock.Verify(c => c.Put("1.key", It.IsAny<object>(), ttl));
        }

        [Test]
        public void Put_Should_EncodeValue()
        {
            _cache.Put("1", "cached value", TimeSpan.FromSeconds(1));

            _cacheMock.Verify(
                c => c.Put(It.IsAny<string>(), "cached value" + ValueTransformer.EncodedSuffix, It.IsAny<TimeSpan>()));
        }

        [Test]
        public void Remove_Should_DecorateKey()
        {
            _cache.Remove("00");

            _cacheMock.Verify(c => c.Remove("00.key"));
        }
    }

    // ReSharper restore InconsistentNaming
}
