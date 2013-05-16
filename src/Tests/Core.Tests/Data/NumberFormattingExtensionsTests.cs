namespace Core.Tests.Data
{
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    [SetCulture("ru-RU")]
    public class NumberFormattingExtensionsTests
    {
        [Test]
        public void Double_AsStr_Should_UserNeutrlaCulture()
        {
            const double d = 1.01;
            d.AsStr().Should().Be("1.01");
        }

        [Test]
        public void Int_AsStr_Should_UserNeutralCulture()
        {
            const int i = 10000;
            i.AsStr().Should().Be("10000");
        }
    }
}