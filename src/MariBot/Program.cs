using MariBot.DI.DiscordNet;
using MariBot.Services.Hosted;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilder, services) =>
    {
        services.Configure<BotOptions>(hostBuilder.Configuration.GetSection(BotOptions.Bot));

        services.AddDiscordNetClient();

        services.AddHostedService<DiscordBotLifetimeService>();
    })
    .Build();

await host.RunAsync();
