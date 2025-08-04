using Ambev.DeveloperEvaluation.Domain.Dtos;

namespace Ambev.DeveloperEvaluation.Domain.Services
{
    /// <summary>
    ///  Represents a service for fetching user data from an external domain.
    ///  This acts as an Anti-Corruption Layer, translating external contracts
    ///  into a format usable by our domain.
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets user information by their unique identifier.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <returns>A DTO with the user's data, or null if the user is not found.</returns>
        Task<ExternalUserDto?> GetUserByIdAsync(Guid userId);
    }
}
