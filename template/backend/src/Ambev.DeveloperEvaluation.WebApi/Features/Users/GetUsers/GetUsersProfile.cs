using AutoMapper;
using Ambev.DeveloperEvaluation.Application.Users.GetUsers; 

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users.GetUsers;

/// <summary>
/// Profile for mapping GetUsers feature requests to commands
/// </summary>
public class GetUsersProfile : Profile
{
    /// <summary>
    /// Initializes the mappings for GetUsers feature
    /// </summary>
    public GetUsersProfile()
    {
        CreateMap<GetUsersResult, GetUsersResponse>();
    }
}
