using IAGenerativa.Embeddings.Configurations;
using IAGenerativa.Embeddings.Data;
using IAGenerativa.Embeddings.Models;
using IAGenerativa.Embeddings.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;
using Pgvector;
using Pgvector.EntityFrameworkCore;

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

app.MapPost(pattern: "v1/products", async (
    CreateProductViewModel model,
    AppDbContext context,
    OllamaApiClient ollamaApiClient) =>
{
    var service = ollamaApiClient.AsTextEmbeddingGenerationService();
    var embeddings = await service.GenerateEmbeddingAsync(model.Category);

    var recomendation = new Recomendation
    {
        Title = model.Title,
        Category = model.Category,
        Embedding = new Vector(embeddings)
    };

    var product = new Product
    {
        Title = model.Title,
        Category = model.Category,
        Summary = model.Summary,
        Description = model.Description
    };

    await context.Recomendations.AddAsync(recomendation);
    await context.Products.AddAsync(product);
    await context.SaveChangesAsync();
});

app.MapPost(pattern: "v1/prompt", async (
    QuestionViewModel model,
    AppDbContext context,
    OllamaApiClient ollamaApiClient) =>
{
    var service = ollamaApiClient.AsTextEmbeddingGenerationService();
    var embeddings = await service.GenerateEmbeddingAsync(model.Prompt);

    var recomendations = await context.Recomendations
        .AsNoTracking()
        .OrderBy(x => x.Embedding.CosineDistance(new Vector(embeddings.ToArray())))
        .Take(3)
        .Select(x => new
        {
            x.Title,
            x.Category
        })
        .ToListAsync();

    return Results.Ok(recomendations);
});

await app.MigrateDatabase();

app.Run();
