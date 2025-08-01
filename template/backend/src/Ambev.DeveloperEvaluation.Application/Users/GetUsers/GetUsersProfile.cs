using AutoMapper;
using Ambev.DeveloperEvaluation.Domain.Entities;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Profile for mapping between User entity and GetUsersResponse
/// </summary>
public class GetUsersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUsers operation
    /// </summary>
    public GetUsersProfile()
    {
        CreateMap<User, GetUsersResult>();
    }
}
