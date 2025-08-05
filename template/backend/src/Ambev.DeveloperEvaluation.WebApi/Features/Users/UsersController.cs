using Ambev.DeveloperEvaluation.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Users;

/// <summary>
/// Controller para expor dados de usuários (clientes) mockados.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserRepository _userRepository;

    public UsersController(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    /// <summary>
    /// Lista todos os usuários (clientes) disponíveis para criar uma venda.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAvailableUsers()
    {
        var users = await _userRepository.GetAllAsync();
        return Ok(users);
    }
}
