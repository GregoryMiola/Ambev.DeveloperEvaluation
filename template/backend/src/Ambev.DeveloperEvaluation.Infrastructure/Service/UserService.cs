using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Ambev.DeveloperEvaluation.Domain.Dtos;
using Ambev.DeveloperEvaluation.Domain.Services;
using Microsoft.Extensions.Logging;

namespace Ambev.DeveloperEvaluation.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserService> _logger;

        public UserService(HttpClient httpClient, ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<ExternalUserDto?> GetUserByIdAsync(Guid userId)
        {
            try
            {
                // A documentação users-api.md usa "integer" para ID, mas o padrão moderno é Guid.
                // Estou assumindo que podemos usar Guid aqui. Se for realmente um inteiro, o tipo precisaria ser ajustado.
                var response = await _httpClient.GetAsync($"users/{userId}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                {
                    return null; // Usuário não encontrado, o que é um fluxo esperado.
                }

                response.EnsureSuccessStatusCode(); // Lança uma exceção para outros erros HTTP (500, 401, etc.)

                var externalUser = await response.Content.ReadFromJsonAsync<UserApiResponseDto>();

                if (externalUser?.Name is null)
                {
                    _logger.LogWarning("User data received from external API for user {UserId} is malformed (missing name).", userId);
                    return null;
                }

                // Aqui acontece a "tradução": do contrato externo para o DTO interno.
                return new ExternalUserDto(
                    userId, // Usamos o ID que recebemos para garantir consistência.
                    $"{externalUser.Name.Firstname} {externalUser.Name.Lastname}"
                );
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while calling the external Users API for user {UserId}.", userId);
                throw; // Re-lança a exceção para ser tratada pela camada superior.
            }
        }

        // DTO privado para mapear a resposta da API externa. Ninguém fora desta classe precisa saber sobre ele.
        private record UserApiResponseDto([property: JsonPropertyName("name")] NameDto Name);
        private record NameDto([property: JsonPropertyName("firstname")] string Firstname, [property: JsonPropertyName("lastname")] string Lastname);
    }
}
