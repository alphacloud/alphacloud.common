namespace Core.Tests.Data
{
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class NumberExtensionsTests
    {
        [TestCase(1.0001, 2.0, 1.0, false)]
        [TestCase(1.0, 2.0, 1.0, true)]
        [TestCase(1.0, 2.0, 2.0, true)]
        public void Double_InRange_Should_CheckIfValueIsInRange(double min, double max, double value, bool result)
        {
            value.InRange(min, max).Should().Be(result);
        }
    }
}