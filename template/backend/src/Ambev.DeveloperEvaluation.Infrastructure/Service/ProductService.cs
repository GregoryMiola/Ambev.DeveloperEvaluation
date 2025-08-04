using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Dtos;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ProductService> _logger;

        public ProductService(HttpClient httpClient, ILogger<ProductService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ExternalProductDto?> GetProductByIdAsync(Guid productId)
        {
            try
            {
                // A documentação Products-api.md usa "integer" para ID, mas o padrão moderno é Guid.
                // Estou assumindo que podemos usar Guid aqui. Se for realmente um inteiro, o tipo precisaria ser ajustado.
                var response = await _httpClient.GetAsync($"products/{productId}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null; // Produto não encontrado, o que é um fluxo esperado.
                }

                response.EnsureSuccessStatusCode(); // Lança uma exceção para outros erros HTTP (500, 401, etc.)

                var externalProduct = await response.Content.ReadFromJsonAsync<ProductApiResponseDto>();

                if (externalProduct?.Title is null)
                {
                    _logger.LogWarning("Product data received from external API for product {ProductId} is malformed (missing name).", productId);
                    return null;
                }

                // Aqui acontece a "tradução": do contrato externo para o DTO interno.
                return new ExternalProductDto(
                    productId, // Usamos o ID que recebemos para garantir consistência.
                    $"{externalProduct.Title}",
                    0 // O preço não é retornado pela API, então usamos 0. Isso pode ser ajustado se necessário.
                );
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while calling the external Products API for product {ProductId}.", productId);
                throw; // Re-lança a exceção para ser tratada pela camada superior.
            }
        }

        // DTO privado para mapear a resposta da API externa. Ninguém fora desta classe precisa saber sobre ele.
        private record ProductApiResponseDto([property: JsonPropertyName("title")] string Title);
    }
}
