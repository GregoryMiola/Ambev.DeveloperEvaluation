namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class ApiResponse
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public IEnumerable<ApiValidationError> Errors { get; set; } = [];
}

public class ApiValidationError
{
    public string? PropertyName { get; set; }
    public string ErrorMessage { get; set; } = string.Empty;
}
