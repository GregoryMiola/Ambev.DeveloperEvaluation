using AutoMapper;
using MediatR;
using FluentValidation;
using Ambev.DeveloperEvaluation.Domain.Repositories;

namespace Ambev.DeveloperEvaluation.Application.Users.GetUsers;

/// <summary>
/// Handler for processing GetUsersCommand requests
/// </summary>
public class GetUsersHandler : IRequestHandler<GetUsersCommand, List<GetUsersResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of GetUserHandler
    /// </summary>
    /// <param name="userRepository">The user repository</param>
    /// <param name="mapper">The AutoMapper instance</param>
    /// <param name="validator">The validator for GetUserCommand</param>
    public GetUsersHandler(
        IUserRepository userRepository,
        IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Handles the GetUsersCommand request
    /// </summary>
    /// <param name="request">The GetUsers command</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <returns>The list of users if found</returns>
    public async Task<List<GetUsersResult>> Handle(GetUsersCommand request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAsync(cancellationToken);        

        return users != null ? _mapper.Map<List<GetUsersResult>>(users) : [];
    }
}
