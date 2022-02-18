using MariBot.DI;
using Microsoft.Extensions.Options;

namespace MariBot.Services.Hosted;

public class DiscordBotLifetimeService : IHostedService
{
    private readonly IMariDiscordClient _discordClient;
    private readonly BotOptions _botOptions;

    public DiscordBotLifetimeService(IMariDiscordClient discordClient, IOptions<BotOptions> botOptions)
    {
        _discordClient = discordClient;
        _botOptions = botOptions.Value;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        return _discordClient.StartAsync(_botOptions.Token, cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return _discordClient.StopAsync(cancellationToken);
    }
}
