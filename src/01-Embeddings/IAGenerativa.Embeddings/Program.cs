using IAGenerativa.Embeddings.Configurations;
using IAGenerativa.Embeddings.Data;
using IAGenerativa.Embeddings.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddDataConfiguration(builder.Configuration)
    .AddOllamaConfiguration(builder.Configuration)
    .AddAppConfiguration(builder.Configuration);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.MapGet("v1/seed", async (AppDbContext dbContext, OllamaApiClient ollamaApiClient) =>
{

    var products = await dbContext.Products.ToListAsync();

    foreach (var product in products)
    {
        var service = ollamaApiClient.AsTextEmbeddingGenerationService();
        var embeddings = await service.GenerateEmbeddingAsync(product.Description);

        var recomendation = new Recomendation
        {
            Title = product.Title,
            Category = product.Category,
            Embedding = new Vector(embeddings)
        };

        await dbContext.Recomendations.AddAsync(recomendation);
        await dbContext.SaveChangesAsync();
    }

    return Results.Ok("Seed data completed.");
});

await app.MigrateDatabase();

app.Run();
