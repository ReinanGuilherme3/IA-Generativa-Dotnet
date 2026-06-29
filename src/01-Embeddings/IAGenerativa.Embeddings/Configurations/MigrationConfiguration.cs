using FluentMigrator.Runner;
using IAGenerativa.Embeddings.Data;
using IAGenerativa.Embeddings.Data.Migrations;
using Microsoft.EntityFrameworkCore;

namespace IAGenerativa.Embeddings.Configurations;

public static class MigrationConfiguration
{
    public static IServiceCollection AddDataConfiguration(this IServiceCollection services, IConfiguration configuration)
    => services
            .AddMigrationConfiguration(configuration)
            .AddAppDbContext(configuration);

    private static IServiceCollection AddMigrationConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("Connection")!;

        services
            .AddFluentMigratorCore()
            .ConfigureRunner(rb => rb
                .AddPostgres()
                .WithGlobalConnectionString(connectionString)
                .ScanIn(typeof(DatabaseMigration).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());

        return services;
    }

    private static IServiceCollection AddAppDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(name: "DefaultConnection");

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseNpgsql(connectionString, o => o.UseVector());
        });

        return services;
    }

    public static async Task MigrateDatabase(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var connectionString = app.Configuration.GetConnectionString("Connection")!;
        DatabaseMigration.Migrate(connectionString, scope.ServiceProvider);
    }
}
