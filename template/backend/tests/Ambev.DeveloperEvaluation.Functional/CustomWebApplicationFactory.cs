using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.Functional;

/// <summary>
/// Factory para criar uma instância da aplicação em memória para testes funcionais.
/// Esta classe é crucial para substituir serviços de produção (como o banco de dados real)
/// por implementações de teste (como um banco de dados em memória).
/// </summary>
public class CustomWebApplicationFactory : WebApplicationFactory<Program> // 'Program' é a classe de entrada da sua WebApi
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // 1. Encontrar e remover a configuração do DbContext de produção (PostgreSQL).
            // Isso é essencial para evitar que o EF Core tente usar duas configurações de DbContext.
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(DbContextOptions<DefaultContext>));

            if (dbContextDescriptor != null)
            {
                services.Remove(dbContextDescriptor);
            }

            // 2. Adicionar um novo DbContext configurado para usar um banco de dados em memória.
            // Usamos um nome de banco de dados único (com Guid) para garantir que cada
            // execução de teste seja isolada e não compartilhe estado com outras.
            services.AddDbContext<DefaultContext>(options =>
            {
                options.UseInMemoryDatabase($"InMemoryDbForTesting-{Guid.NewGuid()}");
            });

            // Aqui você pode adicionar outras substituições de serviço, como IEventPublisher, etc.
        });
    }
}
