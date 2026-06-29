using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;

namespace IAGenerativa.Embeddings.Configurations;

public static class OllamaConfiguration
{
    public static IServiceCollection AddOllamaConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        var ollama = configuration.GetSection("Ollama").Get<OllamaSettings>()!;

        var ollamaClient = new OllamaApiClient(uriString: ollama.Endpoint, defaultModel: ollama.Model);

        services.AddTransient<OllamaApiClient>(x => ollamaClient);
        services.AddSingleton(ollamaClient.AsTextEmbeddingGenerationService());

        return services;
    }
}

public class OllamaSettings
{
    public string Endpoint { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
}
