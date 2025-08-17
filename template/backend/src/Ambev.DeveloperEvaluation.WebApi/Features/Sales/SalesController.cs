﻿using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CreateSale;
using Ambev.DeveloperEvaluation.Application.Queries.Sales.GetSale;
using Ambev.DeveloperEvaluation.Application.Queries.Sales.GetSales;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CancelSale;
using Ambev.DeveloperEvaluation.Application.Commands.Sales.CancelSaleItem;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.WebApi.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller para gerenciar as operações de Venda
/// </summary>
[ApiController]
[Route("api/v1/sales")]
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
       
        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetSaleById), new { id = result.Id }, new ApiResponseWithData<SaleResponse>(result, "Venda criada com sucesso."));
    }
    
    [HttpGet("{id:guid}", Name = "GetSaleById")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSaleById(Guid id)
    {  
        try
        {
            var saleResponse = await _mediator.Send(new GetSaleQuery(id));
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
    public async Task<IActionResult> GetAllSales([FromQuery] GetSalesQuery command)
    {
        try
        {
            var sales = await _mediator.Send(command);
            
            return OkPaginated(sales);
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
            await _mediator.Send(new CancelSaleCommand(id));
            
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
            await _mediator.Send(new CancelSaleItemCommand(saleId, itemId));
            
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
