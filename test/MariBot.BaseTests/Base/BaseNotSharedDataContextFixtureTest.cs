using MariBot.BaseTests.Fixtures;
using Xunit;

namespace MariBot.BaseTests.Base;

public class BaseNotSharedDataContextFixtureTest : IAsyncLifetime
{
    protected readonly DataContextFixture _fixture;

    public BaseNotSharedDataContextFixtureTest()
    {
        _fixture = new DataContextFixture();
    }

    public Task InitializeAsync()
    {
        return _fixture.InitializeAsync();
    }

    public Task DisposeAsync()
    {
        return _fixture.DisposeAsync();
    }
}
