using Ambev.DeveloperEvaluation.Domain.Enums;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Response model for GetUsers operation
/// </summary>
public class GetUsersResult
{
    /// <summary>
    /// The user's full name
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// The user's email address
    /// </summary>
    public string Email { get; set; } = string.Empty;

    /// <summary>
    /// The user's ID 
    /// </summary>
    public Guid Id { get; set; } = Guid.Empty;
}
