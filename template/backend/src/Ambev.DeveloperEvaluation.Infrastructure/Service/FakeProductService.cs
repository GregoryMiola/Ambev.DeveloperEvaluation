using Ambev.DeveloperEvaluation.Domain.Dtos;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Infrastructure.Services
{
    /// <summary>
    /// Uma implementação "fake" do IProductService para uso em desenvolvimento e testes locais.
    /// Retorna dados fixos sem fazer uma chamada HTTP real.
    /// </summary>
    public class FakeProductService : IProductService
    {
        private readonly Dictionary<Guid, ExternalProductDto> _products = new();

        public FakeProductService()
        {
            var productId = Guid.Parse("8d5c5f6a-8b9a-4b0e-8f3f-6a7d8c9b0a1b");
            _products.Add(productId, new ExternalProductDto(productId, "Produto Exemplo", 19.99m));
        }

        public Task<ExternalProductDto?> GetProductByIdAsync(Guid productId)
        {
            _products.TryGetValue(productId, out var product);
            return Task.FromResult(product);
        }
    }
}
