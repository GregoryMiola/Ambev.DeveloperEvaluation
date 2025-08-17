namespace Ambev.DeveloperEvaluation.WebApi.Common;

public class ApiResponseWithData<T> : ApiResponse
{
    public T? Data { get; set; }

    // Adicionamos um construtor padrão para garantir compatibilidade com deserialização e outras inicializações.
    public ApiResponseWithData() { }

    // Este é o construtor que resolve o nosso problema.
    public ApiResponseWithData(T data, string message = "Operação realizada com sucesso.")
    {
        Success = true;
        Data = data;
        Message = message;
    }
}
