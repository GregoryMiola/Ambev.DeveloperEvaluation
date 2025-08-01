using MediatR;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Command for retrieving a list of users
/// </summary>
public record GetUsersCommand : IRequest<List<GetUsersResult>>
{
}
