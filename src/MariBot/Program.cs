using MariBot.Data.Contexts;
using MariBot.Data.Repositories;
using MariBot.DI.DiscordNet;
using MariBot.Services.Hosted;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilder, services) =>
    {
        services.Configure<BotOptions>(hostBuilder.Configuration.GetSection(BotOptions.Bot));

        services.AddPostgresDataContext();
        services.AddRepositories();

        services.AddDiscordNetClient();

        services.AddHostedService<MigrationService>();
        services.AddHostedService<DiscordBotLifetimeService>();
    })
    .Build();

await host.RunAsync();

public class Guild
{
    public ulong Id { get; set; }
    public string? Name { get; set; }
}

public class User
{
    public ulong Id { get; set; }

    public string? Username { get; set; }

    public string? CompleteName { get; set; }

    public string? DisplayName { get; set; }
}

public class Marry
{
    public ulong PartnerId1 { get; set; }

    public ulong PartnerId2 { get; set; }
}
