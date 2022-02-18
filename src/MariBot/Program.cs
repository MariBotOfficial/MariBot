var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilder, services) =>
    {
        services.Configure<BotOptions>(hostBuilder.Configuration.GetSection(BotOptions.Bot));
    })
    .Build();

await host.RunAsync();
