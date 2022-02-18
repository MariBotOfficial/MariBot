namespace MariBot.DI;

public interface IMariDiscordClient
{
    Task StartAsync(string token, CancellationToken cancellationToken);

    Task StopAsync(CancellationToken cancellationToken);
}
