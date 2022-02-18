using Discord;
using Discord.WebSocket;

namespace MariBot.DI.DiscordNet;

public class MariDiscordNetClient : IMariDiscordClient
{
    private readonly DiscordSocketClient _discordSocketClient;

    public MariDiscordNetClient(DiscordSocketClient discordSocketClient)
    {
        _discordSocketClient = discordSocketClient;
    }

    public async Task StartAsync(string token, CancellationToken cancellationToken)
    {
        await _discordSocketClient.LoginAsync(TokenType.Bot, token, false).WaitAsync(cancellationToken);
        await _discordSocketClient.StartAsync().WaitAsync(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _discordSocketClient.SetStatusAsync(UserStatus.Invisible).WaitAsync(cancellationToken);
        await _discordSocketClient.StopAsync().WaitAsync(cancellationToken);
    }
}
