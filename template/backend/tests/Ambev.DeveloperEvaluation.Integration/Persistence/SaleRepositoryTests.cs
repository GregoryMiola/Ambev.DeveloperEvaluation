using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.ORM.Repositories;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Ambev.DeveloperEvaluation.Integration.Persistence;

public class SaleRepositoryTests
{
    /// <summary>
    /// Cria um novo contexto de banco de dados em memória para cada teste.
    /// Usar um nome de banco de dados único (com Guid) garante que cada teste
    /// seja completamente isolado e não interfira nos outros.
    /// </summary>
    private DbContextOptions<DefaultContext> CreateNewContextOptions()
    {
        return new DbContextOptionsBuilder<DefaultContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
    }

    [Fact]
    public async Task AddAsync_And_GetByIdAsync_ShouldPersistAndRetrieveSaleWithItemsCorrectly()
    {
        // Arrange
        var options = CreateNewContextOptions();
        var product = new Product { Id = Guid.NewGuid(), Name = "Test Product", Price = 100m };
        var sale = Sale.Create(Guid.NewGuid(), "Test Customer", Guid.NewGuid());
        sale.AddItem(product, 5); // Adiciona um item com 10% de desconto

        // Act: Salva a venda usando uma instância do contexto.
        // O bloco 'using' garante que a conexão e os recursos sejam liberados.
        using (var context = new DefaultContext(options))
        {
            var repository = new SaleRepository(context);
            await repository.AddAsync(sale);
            // No padrão Unit of Work, o repositório apenas prepara as alterações.
            // O teste precisa simular o "commit" da transação, que seria feito pelo UnitOfWork na camada de aplicação.
            await context.SaveChangesAsync();
        }

        // Assert: Recupera a venda usando uma NOVA instância do contexto.
        // Isso simula uma requisição diferente e garante que os dados foram realmente persistidos.
        using (var context = new DefaultContext(options))
        {
            var repository = new SaleRepository(context);
            var retrievedSale = await repository.GetByIdAsync(sale.Id);

            // Verificações
            Assert.NotNull(retrievedSale);
            Assert.Equal(sale.Id, retrievedSale.Id);
            Assert.Equal("Test Customer", retrievedSale.CustomerName);
            Assert.Single(retrievedSale.Items); // Garante que o item foi salvo junto.
            Assert.Equal(product.Name, retrievedSale.Items.First().ProductName);
            Assert.Equal(450, retrievedSale.TotalAmount); // Verifica se o total foi calculado e salvo corretamente (100 * 5 * 0.9).
        }
    }
}
