using System;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Dtos;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    /// <summary>
    /// Represents a service for fetching product data from an external domain.
    /// </summary>
    public interface IProductService
    {
        Task<ExternalProductDto?> GetProductByIdAsync(Guid productId);
    }
}
