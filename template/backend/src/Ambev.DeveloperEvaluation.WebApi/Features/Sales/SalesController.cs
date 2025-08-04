﻿using Ambev.DeveloperEvaluation.Application.Commands.Sales;
using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Domain.Exceptions;
using Ambev.DeveloperEvaluation.WebApi.Common;
using Microsoft.AspNetCore.Mvc;

namespace Ambev.DeveloperEvaluation.WebApi.Features.Sales;

/// <summary>
/// Controller para gerenciar as operações de Venda
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class SalesController : BaseController
{
    private readonly ISaleService _saleService;
    private readonly ILogger<SalesController> _logger;

    /// <summary>
    /// Initializes a new instance of SalesController
    /// </summary>
    public SalesController(ISaleService saleService, ILogger<SalesController> logger)
    {
        _saleService = saleService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResponse>), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateSale([FromBody] CreateSaleCommand command)
    {
        try
        {
            var saleResponse = await _saleService.CreateSaleAsync(command);
            // Usando o helper do BaseController para retornar 201 Created com o location header correto.
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
    
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(ApiResponseWithData<SaleResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetSaleById(Guid id)
    {  
        try
        {
            var saleResponse = await _saleService.GetSaleByIdAsync(id);
            if (saleResponse == null)
            {
                return NotFound("Venda não encontrada.");
            }
            // Passando apenas os dados para o helper Ok, que fará o encapsulamento.
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
            var sales = await _saleService.GetAllSalesAsync();
            // Passando apenas os dados para o helper Ok.
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
            await _saleService.CancelSaleAsync(id);
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
            await _saleService.RemoveItemFromSaleAsync(saleId, itemId);
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
