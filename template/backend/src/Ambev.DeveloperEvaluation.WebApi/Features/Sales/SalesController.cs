﻿using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller para gerenciar as operações de Venda
/// </summary>
[ApiController]
[Route("api/Sales")] // Rota explícita e versionada
public class SalesController : BaseController
{
    private readonly ILogger<SalesController> _logger;
    private readonly IMediator _mediator;

    /// <summary>
    /// Initializes a new instance of SalesController
    /// </summary>
    public SalesController(ILogger<SalesController> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
    {
        try
        {
            var saleResponse = await _mediator.Send(command);
            _logger.LogInformation($"Venda criada com sucesso. SaleId: {saleResponse.Id}");
            
            return Created(nameof(GetSaleById), new { id = saleResponse.Id }, saleResponse);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning("Falha na regra de negócio ao criar venda: {Message}", ex.Message);
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro inesperado ao criar venda.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "Ocorreu um erro inesperado.", Success = false });
        }
    }
    
    [HttpGet("{id:guid}", Name = "GetSaleById")] // Adicionando um nome à rota
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSaleById(Guid id)
    {  
        try
        {
            var saleResponse = await _mediator.Send(new GetSaleCommand(id));
            if (saleResponse == null)
            {
                return NotFound("Venda não encontrada.");
            }

            return Ok(saleResponse);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar venda por ID.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "Ocorreu um erro inesperado.", Success = false });
        }
    }

    [HttpGet]
    [ProducesResponseType(typeof(ApiResponseWithData<IEnumerable<SaleResponse>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]   
    public async Task<IActionResult> GetAllSales()
    {
        try
        {
            var sales = await _mediator.Send(new GetSalesCommand());
            
            return Ok(sales);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao buscar todas as vendas.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "Ocorreu um erro inesperado.", Success = false });
        }
    }

    [HttpPatch("{id:guid}/cancel")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]  
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CancelSale(Guid id)
    {
        try
        {
            // O serviço já lança uma DomainException se a venda não for encontrada,
            // que será capturada pelo bloco catch. Não precisamos verificar aqui.            
            await _mediator.Send(new CancelSaleCommand(id));
            _logger.LogInformation($"Venda cancelada com sucesso. SaleId: {id}");
            
            return Ok("Venda cancelada com sucesso.");
        }
        catch (DomainException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao cancelar venda.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "Ocorreu um erro inesperado.", Success = false });
        }
    }

    [HttpDelete("{saleId:guid}/items/{itemId:guid}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]  
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> RemoveItemFromSale(Guid saleId, Guid itemId)       
    {
        try
        {
            // O serviço já lança uma DomainException se a venda/item não for encontrado.
            await _mediator.Send(new CancelSaleItemCommand(saleId, itemId));
            _logger.LogInformation($"Item removido da venda com sucesso. SaleId: {SaleId}, ItemId: {ItemId}");
            
            return Ok("Item removido da venda com sucesso.");
        }
        catch (DomainException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao remover item da venda.");
            return StatusCode(StatusCodes.Status500InternalServerError, new ApiResponse { Message = "Ocorreu um erro inesperado.", Success = false });
        }
    }
}
