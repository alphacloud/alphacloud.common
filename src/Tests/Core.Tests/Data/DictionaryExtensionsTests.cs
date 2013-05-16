namespace Core.Tests.Data
{
    using System.Collections.Generic;
    using Alphacloud.Common.Core.Data;
    using FluentAssertions;
    using NUnit.Framework;

    #region using

    #endregion

//// ReSharper disable InconsistentNaming

    [TestFixture]
    internal class DictionaryExtensionsTests
    {
        [Test]
        public void GetOrRetrieve_Should_RetrieveValue_IfItDoesNotExistInDictionary()
        {
            var dic = new SortedList<string, string>();
            dic.GetOrRetrieve("key", k => "new")
                .Should().Be("new");
        }

        [Test]
        public void GetOrRetrieve_Should_TryExistingValueFirst()
        {
            var dic = new SortedList<string, string> {{"key", "existing"}};
            dic.GetOrRetrieve("key", k => "new")
                .Should().Be("existing");
        }
    }

//// ReSharper restore InconsistentNaming
}