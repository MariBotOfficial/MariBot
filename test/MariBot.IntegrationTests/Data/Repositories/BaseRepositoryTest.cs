using System.Linq;
using System.Threading.Tasks;
using MariBot.BaseTests.Base;
using MariBot.Data.Contexts;
using MariBot.Data.Models;
using MariBot.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MariBot.IntegrationTests.Data.Repositories;

public class BaseRepositoryTest : BaseNotSharedDataContextFixtureTest
{
    private class TestRepository : BaseRepository<GuildModel>
    {
        public TestRepository(DataContext context, UnitOfWork unitOfWork) : base(context, unitOfWork)
        {
        }
    }

    private TestRepository GetTestRepository(UnitOfWork? unitOfWork = null)
    {
        return new TestRepository(_fixture.Context, unitOfWork ?? new UnitOfWork(_fixture.Context));
    }

    [Fact]
    public async Task ListAllWhenCallGetAllAsync()
    {
        // Arrange
        var guild1 = new GuildModel(1, "guild1");
        var guild2 = new GuildModel(2, "guild2");

        await _fixture.Context.AddRangeAsync(guild1, guild2);
        _ = await _fixture.Context.SaveChangesAsync();

        var repository = GetTestRepository();

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.NotEmpty(result);
        Assert.Equal(result, new[] { guild1, guild2 });
    }

    [Fact]
    public async Task GetByIdWhenCallGetByIdAsync()
    {
        // Arrange
        var guild1 = new GuildModel(1, "guild1");
        var guild2 = new GuildModel(2, "guild2");

        await _fixture.Context.AddRangeAsync(guild1, guild2);
        _ = await _fixture.Context.SaveChangesAsync();

        var repository = GetTestRepository();

        // Act
        var result = await repository.GetByIdAsync(guild1.Id);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result, guild1);
    }

    [Fact]
    public async Task CreatesWhenCallCreateAsyncAndTransactionIsntOpened()
    {
        // Arrange
        var guild1 = new GuildModel(1, "guild1");

        var repository = GetTestRepository();

        // Act
        _ = await repository.CreateAsync(guild1);
        var result = await repository.Query().AsNoTracking().Where(x => x.Id == guild1.Id).FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
    }

    [Fact]
    public async Task CreatesOnlyWhenCallCreateAsyncAndTransactionIsClosed()
    {
        // Arrange
        var unitOfWork = new UnitOfWork(_fixture.Context);

        var guild1 = new GuildModel(1, "guild1");

        var repository = GetTestRepository(unitOfWork);

        // Act + Assert
        unitOfWork.OpenTransaction();

        _ = await repository.CreateAsync(guild1);
        var result = await repository.Query().AsNoTracking().Where(x => x.Id == guild1.Id).FirstOrDefaultAsync();

        Assert.Null(result);

        _ = await unitOfWork.CommitAsync(true);

        result = await repository.Query().AsNoTracking().Where(x => x.Id == guild1.Id).FirstOrDefaultAsync();

        Assert.NotNull(result);
    }

    [Fact]
    public async Task UpdatesWhenCallUpdateAsyncAndTransactionIsntOpened()
    {
        // Arrange
        var guild1 = new GuildModel(1, "guild1");

        _ = await _fixture.Context.AddAsync(guild1);
        _ = await _fixture.Context.SaveChangesAsync();

        _fixture.Context.ChangeTracker.Clear();

        var repository = GetTestRepository();
        var expectedName = "guild2";

        // Act
        _ = await repository.UpdateAsync(new GuildModel(guild1.Id, expectedName));
        var result = await repository.Query().AsNoTracking().Where(x => x.Id == guild1.Id).FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(result!.Name, expectedName);
    }

    [Fact]
    public async Task UpdatesOnlyWhenCallUpdateAsyncAndTransactionIsClosed()
    {
        // Arrange
        var unitOfWork = new UnitOfWork(_fixture.Context);

        var guild1 = new GuildModel(1, "guild1");

        _ = await _fixture.Context.AddAsync(guild1);
        _ = await _fixture.Context.SaveChangesAsync();

        _fixture.Context.ChangeTracker.Clear();

        var repository = GetTestRepository(unitOfWork);

        var expectedName = "guild2";

        // Act + Assert
        unitOfWork.OpenTransaction();

        _ = await repository.UpdateAsync(new GuildModel(guild1.Id, expectedName));
        var result = await repository.Query().AsNoTracking().Where(x => x.Id == guild1.Id).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.NotEqual(result!.Name, expectedName);

        _ = await unitOfWork.CommitAsync(true);

        result = await repository.Query().AsNoTracking().Where(x => x.Id == guild1.Id).FirstOrDefaultAsync();

        Assert.NotNull(result);
        Assert.Equal(result!.Name, expectedName);
    }
}
