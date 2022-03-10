using MariBot.Data.Models;
using MariBot.Data.Services;

namespace MariBot.Data.Repositories;

public static class RepositoriesInjection
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        _ = services.AddScoped<UnitOfWork>();
        _ = services.AddGuildRepository();

        return services;
    }

    public static IServiceCollection AddGuildRepository(this IServiceCollection services)
    {
        _ = services.AddScoped<GuildRepository>();
        _ = services.AddScoped<BaseRepository<GuildModel>>(sp =>
        {
            return sp.GetRequiredService<GuildRepository>();
        });

        return services;
    }
}
