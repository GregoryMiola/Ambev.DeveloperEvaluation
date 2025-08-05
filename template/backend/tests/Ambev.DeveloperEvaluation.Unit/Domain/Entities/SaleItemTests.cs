using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Domain.Entities;

public class SaleItemTests
{
    // Arrange: Criamos um produto de exemplo que será usado em todos os testes.
    private readonly Product _sampleProduct = new() { Id = Guid.NewGuid(), Name = "Test Product", Price = 100m };

    [Fact]
    public void Create_WithQuantityLessThan4_ShouldApplyZeroDiscount()
    {
        // Arrange
        var quantity = 3;
        var expectedDiscount = 0m;
        var expectedTotal = _sampleProduct.Price * quantity; // 100 * 3 = 300

        // Act
        var saleItem = SaleItem.Create(Guid.NewGuid(), _sampleProduct, quantity);

        // Assert
        Assert.Equal(expectedDiscount, saleItem.DiscountPercentage);
        Assert.Equal(expectedTotal, saleItem.TotalItemAmount);
    }

    // Usamos [Theory] e [InlineData] para testar múltiplos valores com o mesmo teste,
    // tornando nosso código mais enxuto e eficiente.
    [Theory]
    [InlineData(4)]  // Testando o limite inferior
    [InlineData(7)]  // Testando um valor no meio
    [InlineData(9)]  // Testando o limite superior
    public void Create_WithQuantityBetween4And9_ShouldApply10PercentDiscount(int quantity)
    {
        // Arrange
        var expectedDiscount = 10m;
        var expectedTotal = (_sampleProduct.Price * quantity) * (1 - (expectedDiscount / 100m));

        // Act
        var saleItem = SaleItem.Create(Guid.NewGuid(), _sampleProduct, quantity);

        // Assert
        Assert.Equal(expectedDiscount, saleItem.DiscountPercentage);
        Assert.Equal(expectedTotal, saleItem.TotalItemAmount);
    }

    [Theory]
    [InlineData(10)] // Testando o limite inferior
    [InlineData(15)] // Testando um valor no meio
    [InlineData(20)] // Testando o limite superior
    public void Create_WithQuantityBetween10And20_ShouldApply20PercentDiscount(int quantity)
    {
        // Arrange
        var expectedDiscount = 20m;
        var expectedTotal = (_sampleProduct.Price * quantity) * (1 - (expectedDiscount / 100m));

        // Act
        var saleItem = SaleItem.Create(Guid.NewGuid(), _sampleProduct, quantity);

        // Assert
        Assert.Equal(expectedDiscount, saleItem.DiscountPercentage);
        Assert.Equal(expectedTotal, saleItem.TotalItemAmount);
    }

    [Fact]
    public void Create_WithQuantityGreaterThan20_ShouldThrowDomainException()
    {
        // Arrange
        var quantity = 21;

        // Act & Assert
        // Verificamos se uma exceção do tipo DomainException é lançada
        // e se a mensagem de erro contém o texto esperado.
        var exception = Assert.Throws<DomainException>(() => SaleItem.Create(Guid.NewGuid(), _sampleProduct, quantity));
        Assert.Contains("Não é possível vender mais de 20 itens", exception.Message);
    }
}
