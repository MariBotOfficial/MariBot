using Microsoft.EntityFrameworkCore;

namespace MariBot.Data.Contexts;

public static class PostgresDataContextInjection
{
    public static IServiceCollection AddPostgresDataContext(this IServiceCollection services)
    {
        _ = services.AddDbContext<DataContext>((sp, options) =>
        {
            if (sp.GetRequiredService<IHostEnvironment>().IsDevelopment())
            {
                _ = options.EnableDetailedErrors();
                _ = options.EnableSensitiveDataLogging();
            }

            _ = options.UseApplicationServiceProvider(sp);
            _ = options.UseLoggerFactory(sp.GetRequiredService<ILoggerFactory>());

            _ = options.UseNpgsql(sp.GetRequiredService<IConfiguration>().GetConnectionString("Postgres"), postgresOptions =>
            {
                _ = postgresOptions.UseRelationalNulls();
            });
        });

        return services;
    }
}
