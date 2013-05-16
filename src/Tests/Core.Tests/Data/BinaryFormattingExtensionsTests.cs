namespace Core.Tests.Data
{
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class BinaryFormattingExtensionsTests
    {
        [Test]
        public void Byte_AsBinary_ShouldFormatAsBinary()
        {
            const byte b = 100;
            b.AsBinary().Should().Be("01100100");
        }
    }
}