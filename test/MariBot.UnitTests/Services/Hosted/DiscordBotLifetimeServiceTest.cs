using System.Threading;
using System.Threading.Tasks;
using MariBot.DI;
using MariBot.Services.Hosted;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace MariBot.UnitTests.Services.Hosted;

public class DiscordBotLifetimeServiceTest
{
    private static IOptions<BotOptions> GetOptions()
    {
        var optionsMock = new Mock<IOptions<BotOptions>>();

        _ = optionsMock.SetupGet(x => x.Value).Returns(new BotOptions
        {
            Token = "token"
        });

        return optionsMock.Object;
    }

    private static Mock<IMariDiscordClient> GetDiscordClientMock()
    {
        var discordClientMock = new Mock<IMariDiscordClient>();

        _ = discordClientMock.Setup(x => x.StartAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()));
        _ = discordClientMock.Setup(x => x.StopAsync(It.IsAny<CancellationToken>()));

        return discordClientMock;
    }

    [Fact]
    public async Task ShouldCallDiscordStartWhenCallStart()
    {
        // Arrange
        var discordClientMock = GetDiscordClientMock();

        var discordBotLifetimeService = new DiscordBotLifetimeService(discordClientMock.Object, GetOptions());

        // Act
        await discordBotLifetimeService.StartAsync(CancellationToken.None);

        // Assert
        discordClientMock.Verify(x => x.StartAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task ShouldCallDiscordStopWhenCallStop()
    {
        // Arrange
        var discordClientMock = GetDiscordClientMock();

        var discordBotLifetimeService = new DiscordBotLifetimeService(discordClientMock.Object, GetOptions());

        // Act
        await discordBotLifetimeService.StopAsync(CancellationToken.None);

        // Assert
        discordClientMock.Verify(x => x.StopAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
