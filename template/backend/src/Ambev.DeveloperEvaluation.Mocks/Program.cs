var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Simula o endpoint GET /users/{id} conforme a documentação users-api.md
app.MapGet("/users/{id}", (string id) =>
{
	// Log para vermos no console que o mock foi chamado
	Console.WriteLine($"--> [MOCK] Received request for user ID: {id}");
	// Não importa o ID, sempre retornamos um usuário mockado com a estrutura correta.
	var mockUser = new
	{
		id, // Retorna o mesmo ID que foi solicitado
		name = new { firstname = "John", lastname = "Doe" },
		email = "john.doe.mock@example.com",
		username = "johndoemock"
	};
	return Results.Ok(mockUser);
});

// Simula o endpoint GET /products/{id}
app.MapGet("/products/{id}", (string id) =>
{
    Console.WriteLine($"--> [MOCK] Received request for product ID: {id}");

    // Retorna um produto mockado com preço fixo.
    var mockProduct = new
    {
        id,
        name = "Cerveja Mockada",
        price = 5.50m
    };

    return Results.Ok(mockProduct);
});

app.Run();
