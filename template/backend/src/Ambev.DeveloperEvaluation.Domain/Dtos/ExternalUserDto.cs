using System;

namespace Ambev.DeveloperEvaluation.Domain.Dtos
{
    /// <summary>
    /// Represents the data transfer object for a user retrieved from an external service.
    /// This DTO contains only the information necessary for the Sales domain.
    /// </summary>
    public record ExternalUserDto(
        Guid Id,
        string FullName
    );
}
