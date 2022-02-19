using MariBot.Data.Contexts;
using Microsoft.EntityFrameworkCore;

namespace MariBot.Services.Hosted;

public class MigrationService : IHostedService
{
    private readonly IHostEnvironment _environment;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public MigrationService(IServiceScopeFactory serviceScopeFactory, IHostEnvironment environment)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _environment = environment;
    }

    private CancellationTokenSource? _cts;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        // if (_environment.IsDevelopment())
        // {
        //     return Task.CompletedTask;
        // }

        await using var scope = _serviceScopeFactory.CreateAsyncScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

        _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

        await dbContext.Database.MigrateAsync(_cts.Token);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _cts?.Cancel(false);
        return Task.CompletedTask;
    }
}
