﻿using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.WebApi.Common;

[Route("api/[controller]")]
[ApiController]
public class BaseController : ControllerBase
{
    protected Guid? GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (Guid.TryParse(userIdClaim, out var userId))
        {
            return userId;
        }

        return null;
    }

    /// <summary>
    /// Gets the current user's email from the claims.
    /// </summary>
    /// <returns>The user's email if the claim exists; otherwise, null.</returns>
    protected string? GetCurrentUserEmail() =>
        User.FindFirst(ClaimTypes.Email)?.Value;

    protected IActionResult Ok<T>(T data) =>
            base.Ok(new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
        base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

    protected IActionResult BadRequest(string message) =>
        base.BadRequest(new ApiResponse { Message = message, Success = false });

    protected IActionResult NotFound(string message = "Resource not found") =>
        base.NotFound(new ApiResponse { Message = message, Success = false });

    protected IActionResult OkPaginated<T>(PaginatedList<T> pagedList) =>
            Ok(new PaginatedResponse<T>
            {
                Data = pagedList,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                TotalCount = pagedList.TotalCount,
                Success = true
            });
}
