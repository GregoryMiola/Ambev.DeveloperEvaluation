using Ambev.DeveloperEvaluation.Application.Interfaces;

namespace Ambev.DeveloperEvaluation.Mocks.Repositories;

public class InMemoryUserRepository : IUserRepository
{
    private static readonly List<UserDto> _users = new()
    {
        new UserDto
        {
            Id = Guid.Parse("a4b2c3d4-e5f6-a7b8-c9d0-e1f2a3b4c5d6"),
            Name = "Gregorix"
        },
        new UserDto
        {
            Id = Guid.Parse("c7d8e9f0-a1b2-c3d4-e5f6-a7b8c9d0e1f2"),
            Name = "Ambev GTH"
        },
        new UserDto
        {
            Id = Guid.Parse("b1c2d3e4-f5a6-b7c8-d9e0-f1a2b3c4d5e6"),
            Name = "Cliente VIP"
        },
        new UserDto
        {
            Id = Guid.Parse("e6f5d4c3-b2a1-0f9e-8d7c-6b5a4f3e2d1c"),
            Name = "Visitante Ocasional"
        }
    };

    public Task<UserDto?> FindByIdAsync(Guid userId)
    {
        var user = _users.FirstOrDefault(u => u.Id == userId);
        return Task.FromResult(user);
    }

    public Task<IEnumerable<UserDto>> GetAllAsync()
    {
        return Task.FromResult(_users.AsEnumerable());
    }
}
