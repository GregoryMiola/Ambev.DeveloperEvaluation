﻿using Ambev.DeveloperEvaluation.Domain.Services;
using Ambev.DeveloperEvaluation.ORM;
using Ambev.DeveloperEvaluation.Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class InfrastructureModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<DbContext>(provider => provider.GetRequiredService<DefaultContext>());

        // Agora que temos uma API de Mock, podemos registrar o UserService real
        // para ser usado em todos os ambientes. Ele usará a URL configurada
        // no appsettings.json, que aponta para o nosso serviço de Mock.
        builder.Services.AddHttpClient<IUserService, UserService>(client =>
        {
            var baseUrl = builder.Configuration["ExternalServices:UsersApiUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("A URL da API de Usuários (UsersApiUrl) não está configurada.");
            }
            client.BaseAddress = new Uri(baseUrl);
        });

        // Registra o ProductService da mesma forma.
        builder.Services.AddHttpClient<IProductService, ProductService>(client =>
        {
            // Reutilizamos a mesma URL do mock, pois ambos os endpoints estão lá.
            var baseUrl = builder.Configuration["ExternalServices:UsersApiUrl"];
            if (string.IsNullOrEmpty(baseUrl))
            {
                throw new InvalidOperationException("A URL da API de Usuários (UsersApiUrl) não está configurada.");
            }
            client.BaseAddress = new Uri(baseUrl);
        });
    }
}