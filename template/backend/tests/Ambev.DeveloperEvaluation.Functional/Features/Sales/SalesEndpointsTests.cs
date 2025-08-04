using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Application.Commands.SaleItems;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Mocks.Repositories;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentAssertions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Sales;

public class SalesEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public SalesEndpointsTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateSale_WithValidData_ShouldReturnCreatedAndSaleData()
    {
        // Arrange
        // Pegamos IDs de usuários e produtos que sabemos que existem nos nossos mocks.
        var existingUser = new InMemoryUserRepository().GetAllAsync().Result.First();
        var existingProduct = new InMemoryProductRepository().GetAllAsync().Result.First();

        var command = new CreateSaleCommand(
            existingUser.Id,
            Guid.NewGuid(), // BranchId pode ser qualquer um
            new List<SaleItemCommand> { new() { ProductId = existingProduct.Id, Quantity = 5 } }
        );

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/sales", command);

        // Assert
        // 1. Verificamos se a resposta foi um sucesso e o status code é 201 Created.
        response.EnsureSuccessStatusCode();
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        // 2. Verificamos se o cabeçalho "Location" foi retornado, apontando para o novo recurso.
        response.Headers.Location.Should().NotBeNull();

        // 3. Verificamos o corpo da resposta.
        var apiResponse = await response.Content.ReadFromJsonAsync<ApiResponseWithData<SaleResponse>>();
        apiResponse.Should().NotBeNull();
        apiResponse!.Success.Should().BeTrue();
        apiResponse.Data!.Items.Should().HaveCount(1);
        apiResponse.Data!.Items.First().ProductName.Should().Be(existingProduct.Name);

        // O valor esperado deve ser calculado dinamicamente com base no preço real do produto mockado.
        var expectedTotal = (existingProduct.Price * 5) * 0.9m; // 10% de desconto para 5 itens
        apiResponse.Data!.TotalAmount.Should().Be(expectedTotal);
    }
}
