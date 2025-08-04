using System;

namespace Ambev.DeveloperEvaluation.Domain.Dtos
{
    /// <summary>
    /// Represents the data transfer object for a product retrieved from an external service.
    /// </summary>
    public record ExternalProductDto(
        Guid Id,
        string Title,
        decimal Price
    );
}
