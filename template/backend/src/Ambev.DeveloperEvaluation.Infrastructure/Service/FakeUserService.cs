using Ambev.DeveloperEvaluation.Domain.Dtos;
using Ambev.DeveloperEvaluation.Domain.Services;

namespace Ambev.DeveloperEvaluation.Infrastructure.Services
{
    /// <summary>
    /// Uma implementação "fake" do IUserService para uso em desenvolvimento e testes locais.
    /// Retorna dados fixos sem fazer uma chamada HTTP real.
    /// </summary>
    public class FakeUserService : IUserService
    {
        private readonly Dictionary<Guid, ExternalUserDto> _users = new();

        public FakeUserService()
        {
            var userId = Guid.Parse("8d5c5f6a-8b9a-4b0e-8f3f-6a7d8c9b0a1b");
            _users.Add(userId, new ExternalUserDto(userId, "John Doe (Fake)"));
        }

        public Task<ExternalUserDto?> GetUserByIdAsync(Guid userId)
        {
            _users.TryGetValue(userId, out var user);
            return Task.FromResult(user);
        }
    }
}
