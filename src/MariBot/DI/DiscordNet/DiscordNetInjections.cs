using Discord;
using Discord.WebSocket;
using Microsoft.Extensions.Options;

namespace MariBot.DI.DiscordNet;

public static class DiscordNetInjections
{
    public static IServiceCollection AddDiscordNetClient(this IServiceCollection services)
    {
        _ = services.AddHostedService<DiscordToMicrosoftLoggingService>();

        _ = services.Configure<DiscordSocketConfig>(config =>
        {
            config.AlwaysDownloadUsers = false;

            config.GatewayIntents = GatewayIntents.None;

            // ASP.NET Core logging will decide the correct.
            config.LogLevel = LogSeverity.Debug;
        });

        _ = services.AddSingleton(sp =>
        {
            var config = sp.GetRequiredService<IOptions<DiscordSocketConfig>>();

            return new DiscordSocketClient(config.Value);
        });

        _ = services.AddSingleton<IMariDiscordClient, MariDiscordNetClient>();

        return services;
    }
}
