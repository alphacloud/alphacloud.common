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

// ReSharper disable HeapView.ObjectAllocation.Evident
// ReSharper disable HeapView.BoxingAllocation
// ReSharper disable HeapView.ObjectAllocation
namespace Core.Tests.Data
{
    using Alphacloud.Common.Core.Data.Hash;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class Murmur3Tests
    {
        readonly byte[] _bytes = {00, 01, 02, 03, 04, 05, 06, 07, 08, 09, 0x0a, 0x0b, 0x0c, 0x0d, 0x0e, 0x0f};
        readonly uint[] _dwords = {0x03020100, 0x07060504, 0x0b0a0908, 0x0f0e0d0c};
        readonly int[] _ints = {0x03020100, 0x07060504, 0x0b0a0908, 0x0f0e0d0c};
        ISimpleHasher _hash;


        [SetUp]
        public void SetUp()
        {
            _hash = FastHashAlgorithms.Murmur3;
        }


        [Test]
        public void ShouldComputeHash()
        {
            const int hash = 0x28953b79;
            _hash.Compute(_bytes, 0, _bytes.Length)
                .Should().Be(hash, "Original hash algorith failed");

            _hash.Compute(_ints)
                .Should().Be(hash, "int[] version failed");
        }
    }
}