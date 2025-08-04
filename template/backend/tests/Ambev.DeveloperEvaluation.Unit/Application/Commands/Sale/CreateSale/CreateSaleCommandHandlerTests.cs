using Ambev.DeveloperEvaluation.Application.Commands.SaleItems;
using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Entities;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.Domain.Interfaces;
using AutoMapper;
using NSubstitute;
using Xunit;

namespace Ambev.DeveloperEvaluation.Unit.Application.Commands.Sales.CreateSale;

public class CreateSaleCommandHandlerTests
{
    private readonly IUserRepository _userRepositoryMock;
    private readonly IProductRepository _productRepositoryMock;
    private readonly ISaleRepository _saleRepositoryMock;
    private readonly IMapper _mapperMock;
    private readonly CreateSaleCommandHandler _handler;

    public CreateSaleCommandHandlerTests()
    {
        // Arrange (Global): Criamos "dublês" (mocks) para todas as dependências do nosso handler.
        // O NSubstitute cria um objeto falso que implementa a interface, permitindo que a gente
        // controle seu comportamento e verifique se seus métodos foram chamados.
        _userRepositoryMock = Substitute.For<IUserRepository>();
        _productRepositoryMock = Substitute.For<IProductRepository>();
        _saleRepositoryMock = Substitute.For<ISaleRepository>();
        _mapperMock = Substitute.For<IMapper>();

        // Instanciamos o handler que vamos testar, injetando os mocks.
        _handler = new CreateSaleCommandHandler(
            _userRepositoryMock,
            _productRepositoryMock,
            _saleRepositoryMock,
            _mapperMock
        );
    }

    [Fact]
    public async Task Handle_WithValidCommand_ShouldCreateSaleAndReturnSaleResponse()
    {
        // Arrange
        var command = new CreateSaleCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<SaleItemCommand> { new() { ProductId = Guid.NewGuid(), Quantity = 5 } }
        );

        var userDto = new UserDto { Id = command.CustomerId, Name = "Test User" };
        var productDto = new ProductDto { Id = command.Items.First().ProductId, Name = "Test Product", Price = 100m };
        
        // Configuramos o comportamento dos mocks:
        // "Quando FindByIdAsync for chamado com o ID do cliente do comando, retorne o userDto"
        _userRepositoryMock.FindByIdAsync(command.CustomerId).Returns(Task.FromResult<UserDto?>(userDto));
        _productRepositoryMock.FindByIdAsync(command.Items.First().ProductId).Returns(Task.FromResult<ProductDto?>(productDto));

        // "Quando o Mapper for chamado para mapear qualquer objeto Sale, retorne um SaleResponse"
        _mapperMock.Map<SaleResponse>(Arg.Any<Sale>()).Returns(new SaleResponse { Id = Guid.NewGuid() });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        // Verificamos se o resultado não é nulo.
        Assert.NotNull(result);
        
        // A verificação mais importante: garantimos que o método para salvar a venda foi chamado
        // exatamente uma vez. Isso prova que a orquestração do handler funcionou.
        await _saleRepositoryMock.Received(1).AddAsync(Arg.Any<Sale>());
    }

    [Fact]
    public async Task Handle_WhenUserNotFound_ShouldThrowDomainException()
    {
        // Arrange
        var command = new CreateSaleCommand(
            Guid.NewGuid(),
            Guid.NewGuid(),
            new List<SaleItemCommand> { new() { ProductId = Guid.NewGuid(), Quantity = 5 } }
        );

        // Configuramos o mock para simular o cenário de falha:
        // "Quando FindByIdAsync for chamado, retorne nulo, simulando um cliente não encontrado."
        _userRepositoryMock.FindByIdAsync(command.CustomerId).Returns(Task.FromResult<UserDto?>(null));

        // Act & Assert
        // Verificamos se o handler lança a exceção esperada quando o cliente não existe.
        var exception = await Assert.ThrowsAsync<DomainException>(() => _handler.Handle(command, CancellationToken.None));
        
        Assert.Contains("Cliente com ID", exception.Message);
    }
}
