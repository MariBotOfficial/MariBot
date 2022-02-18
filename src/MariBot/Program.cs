var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((hostBuilder, services) =>
    {
        services.AddOptions<BotOptions>().Bind(hostBuilder.Configuration.GetSection("BotOptions"));
    })
    .Build();

await host.RunAsync();
