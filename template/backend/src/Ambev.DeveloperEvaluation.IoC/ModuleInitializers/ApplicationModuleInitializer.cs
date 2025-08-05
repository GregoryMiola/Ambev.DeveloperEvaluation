using Ambev.DeveloperEvaluation.Application.Interfaces;
using Ambev.DeveloperEvaluation.Application.Mappings;
using Ambev.DeveloperEvaluation.Application.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Ambev.DeveloperEvaluation.IoC.ModuleInitializers;

public class ApplicationModuleInitializer : IModuleInitializer
{
    public void Initialize(WebApplicationBuilder builder)
    {
        // Registra o AutoMapper, procurando por perfis no assembly do SaleMappingProfile.
        builder.Services.AddAutoMapper(typeof(SaleMappingProfile).Assembly);
        // Registra o MediatR, procurando por handlers no mesmo assembly.
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(SaleMappingProfile).Assembly));
        // Registra nossa implementação de publicador de eventos.
        builder.Services.AddScoped<IEventPublisher, LoggingEventPublisher>();
    }
}