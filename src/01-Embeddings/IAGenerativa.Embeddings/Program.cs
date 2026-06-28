using FluentMigrator.Runner;
using IAGenerativa.Embeddings.Data.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .Build();

var connectionString = configuration.GetConnectionString("Connection")!;

var services = new ServiceCollection()
    .AddFluentMigratorCore()
    .ConfigureRunner(rb => rb
        .AddPostgres()
        .WithGlobalConnectionString(connectionString)
        .ScanIn(typeof(DatabaseMigration).Assembly).For.Migrations())
    .AddLogging(lb => lb.AddFluentMigratorConsole())
    .BuildServiceProvider();

await MigrateDatabase();

using var ollamaClient = new OllamaApiClient(
    uriString: "http://localhost:11434",
    defaultModel: "mxbai-embed-large");

var service = ollamaClient.AsTextEmbeddingGenerationService();
var embeddings = await service.GenerateEmbeddingsAsync([
    "café brasileiro tradicional",
    "café brasileiro moderno",
    ]);

foreach (var item in embeddings)
    Console.WriteLine(item);

async Task MigrateDatabase()
{
    await using var scope = services.CreateAsyncScope();
    DatabaseMigration.Migrate(connectionString, scope.ServiceProvider);
}
