using System.Threading.Tasks;
using MariBot.BaseTests.Base;
using MariBot.Data.Models;
using MariBot.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace MariBot.IntegrationTests.Data.Repositories;

public class UnitOfWorkTest : BaseNotSharedDataContextFixtureTest
{
    private UnitOfWork GetUnitOfWork()
    {
        return new UnitOfWork(_fixture.Context);
    }

    [Fact]
    public async Task CommitsChangesWhenCallCommitAsync()
    {
        // Arrange
        var unitOfWork = GetUnitOfWork();

        var guild1 = new GuildModel(1, "guild1");
        _ = await _fixture.Context.Set<GuildModel>().AddAsync(guild1);

        // Act
        _ = await unitOfWork.CommitAsync();

        // Assert
        var result = await _fixture.Context.Set<GuildModel>()
                                            .AsNoTracking()
                                            .FirstAsync();

        Assert.Equal(guild1.Name, result.Name);
    }

    [Fact]
    public async Task RollbackChangesWhenCallRollbackAsync()
    {
        // Arrange
        var expectedName = "guild1";

        var unitOfWork = GetUnitOfWork();

        var guild1 = new GuildModel(1, expectedName);

        _ = await _fixture.Context.Set<GuildModel>().AddAsync(guild1);

        _ = await unitOfWork.CommitAsync();

        // Act
        var guild1Copy = await _fixture.Context
                                            .Set<GuildModel>()
                                            .AsNoTracking()
                                            .FirstAsync();

        guild1Copy.Name = "guild2";

        await unitOfWork.RollBackAsync();

        // Assert
        var result = await _fixture.Context
                                    .Set<GuildModel>()
                                    .AsTracking()
                                    .FirstAsync();

        Assert.Equal(expectedName, result.Name);
    }
}
