using MariBot.Data.Contexts;
using MariBot.Data.Repositories;
using MariBot.DI.DiscordNet;
using MariBot.Services.Hosted;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilder, services) =>
    {
        _ = services.Configure<BotOptions>(hostBuilder.Configuration.GetSection(BotOptions.Bot));

        _ = services.AddPostgresDataContext();
        _ = services.AddRepositories();

        _ = services.AddDiscordNetClient();

        _ = services.AddHostedService<MigrationService>();
        _ = services.AddHostedService<DiscordBotLifetimeService>();
    })
    .Build();

await host.RunAsync();
