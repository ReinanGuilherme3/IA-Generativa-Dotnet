using IAGenerativa.Embeddings.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddMigrationConfiguration(builder.Configuration);
builder.Services.AddOllamaConfiguration(builder.Configuration);
builder.Services.AddAppConfiguration(builder.Configuration);

var app = builder.Build();

await app.MigrateDatabase();

app.Run();
