using FluentMigrator.Runner;
using IAGenerativa.Embeddings.Data.Migrations;

namespace IAGenerativa.Embeddings.Configurations;

public static class MigrationConfiguration
{
    public static IServiceCollection AddMigrationConfiguration(this IServiceCollection services, IConfiguration configuration)
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

    public static async Task MigrateDatabase(this WebApplication app)
    {
        await using var scope = app.Services.CreateAsyncScope();
        var connectionString = app.Configuration.GetConnectionString("Connection")!;
        DatabaseMigration.Migrate(connectionString, scope.ServiceProvider);
    }
}
