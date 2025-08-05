using Ambev.DeveloperEvaluation.Application.Dtos;

namespace Ambev.DeveloperEvaluation.Application.Interfaces;

public interface IUserRepository
{
    Task<UserDto?> FindByIdAsync(Guid userId);
    Task<IEnumerable<UserDto>> GetAllAsync();
}
