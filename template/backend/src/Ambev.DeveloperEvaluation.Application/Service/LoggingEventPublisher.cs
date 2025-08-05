using Ambev.DeveloperEvaluation.Application.Interfaces;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace Ambev.DeveloperEvaluation.Application.Services;

public class LoggingEventPublisher : IEventPublisher
{
    private readonly ILogger<LoggingEventPublisher> _logger;

    public LoggingEventPublisher(ILogger<LoggingEventPublisher> logger)
    {
        _logger = logger;
    }

    public Task PublishAsync(string eventName, object eventData)
    {
        var eventJson = JsonSerializer.Serialize(eventData, new JsonSerializerOptions { WriteIndented = true });
        _logger.LogInformation("----- Publishing Event: {EventName} -----\n{EventPayload}", eventName, eventJson);
        return Task.CompletedTask;
    }
}
