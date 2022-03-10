using MariBot.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace MariBot.BaseTests.Fixtures;

public class DataContextFixture : IAsyncLifetime
{
    public DataContextFixture()
    {
    }

    public string DatabaseName { get; set; } = $"maribot_test_{Guid.NewGuid()}";

    public DataContext Context { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(DatabaseName, options =>
            {
                _ = options.EnableNullChecks();
                ApplyInMemoryDatabaseOptions(options);
            })
            .UseSnakeCaseNamingConvention();

        ApplyConfiguration(optionsBuilder);

        Context = new DataContext(optionsBuilder.Options);

        _ = await Context.Database.EnsureCreatedAsync();

        await SeedAsync();

        _ = await Context.SaveChangesAsync();
    }

    protected virtual void ApplyConfiguration(DbContextOptionsBuilder<DataContext> optionsBuilder)
    {
    }

    protected virtual void ApplyInMemoryDatabaseOptions(InMemoryDbContextOptionsBuilder options)
    {

    }

    protected virtual Task SeedAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Context.DisposeAsync().AsTask();
    }
}
