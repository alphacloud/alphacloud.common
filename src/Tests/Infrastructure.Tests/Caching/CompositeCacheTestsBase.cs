namespace Infrastructure.Tests.Caching
{
    using Alphacloud.Common.Infrastructure.Caching;
    using Alphacloud.Common.Testing.Nunit;
    using Moq;

    abstract class CompositeCacheTestsBase: MockedTestsBase
    {
        Mock<ICache> _localCache;
        Mock<ICache> _remoteCache;

        protected Mock<ICache> LocalCache
        {
            get { return _localCache; }
        }

        protected Mock<ICache> RemoteCache
        {
            get { return _remoteCache; }
        }

        protected override void DoSetup()
        {
            _localCache = Mockery.Create<ICache>();
            _remoteCache = Mockery.Create<ICache>();
        }

        protected override void DoTearDown()
        {
        }
    }
}