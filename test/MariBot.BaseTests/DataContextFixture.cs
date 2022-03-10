using MariBot.Data.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Xunit;

namespace MariBot.BaseTests;

public class DataContextFixture : IAsyncLifetime
{
    private readonly string _databaseName;

    public DataContextFixture() : this($"maribot_test_{Guid.NewGuid()}")
    {
    }

    public DataContextFixture(string databaseName)
    {
        _databaseName = databaseName;
    }

    public DataContext Context { get; private set; } = null!;

    public async Task InitializeAsync()
    {
        var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
            .UseInMemoryDatabase(_databaseName, options =>
            {
                _ = options.EnableNullChecks();
                ApplyInMemoryDatabaseOptions(options);
            })
            .UseSnakeCaseNamingConvention();

        ApplyConfiguration(optionsBuilder);

        Context = new DataContext(optionsBuilder.Options);

        await Context.Database.MigrateAsync();

        await SeedAsync();

        _ = await Context.SaveChangesAsync();
    }

    public virtual void ApplyConfiguration(DbContextOptionsBuilder<DataContext> optionsBuilder)
    {
    }

    public virtual void ApplyInMemoryDatabaseOptions(InMemoryDbContextOptionsBuilder options)
    {

    }

    public virtual Task SeedAsync()
    {
        return Task.CompletedTask;
    }

    public Task DisposeAsync()
    {
        return Context.DisposeAsync().AsTask();
    }
}
