/*using System.Net;
using System.Net.Http.Json;
using Ambev.DeveloperEvaluation.Application.Commands.SaleItems;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Dtos;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.WebApi.Common;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Ambev.DeveloperEvaluation.Functional.Features.Sales;

public class SalesEndpointsTests : IClassFixture<CustomWebApplicationFactory>
{/*
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory _factory;

    public SalesEndpointsTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateSale_WithValidData_ShouldReturnCreatedAndSaleData()
    {
        // Arrange
        // To ensure we use the same in-memory data as the running application,
        // we must get the repository instances from the factory's service provider.
        // Creating `new InMemory...Repository()` here would create isolated instances
        // with no data, causing the application to fail when looking up users/products.
        await using var scope = _factory.Services.CreateAsyncScope();
        var userRepository = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        var productRepository = scope.ServiceProvider.GetRequiredService<IProductRepository>();
        var existingUser = (await userRepository.GetAllAsync()).First();
        var existingProduct = (await productRepository.GetAllAsync()).First();

        var command = new CreateSaleCommand(
            existingUser.Id,
            Guid.NewGuid(), // BranchId pode ser qualquer um
            new List<SaleItemCommand> { new SaleItemCommand(existingProduct.Id, 5) }
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
}*/
