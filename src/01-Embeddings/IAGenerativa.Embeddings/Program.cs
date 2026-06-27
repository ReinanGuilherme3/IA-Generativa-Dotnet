using Microsoft.SemanticKernel.Embeddings;
using OllamaSharp;

Console.WriteLine("Hello, World!");

using var ollamaClient = new OllamaApiClient(
    uriString: "http://localhost:11434",
    defaultModel: "mxbai-embed-large");

var service = ollamaClient.AsTextEmbeddingGenerationService();
var embeddings = await service.GenerateEmbeddingsAsync([
    "café brasileiro tradicional",
    "café brasileiro moderno",
    ]);

foreach (var item in embeddings)
{
    Console.WriteLine(item);
}