namespace Ambev.DeveloperEvaluation.Application.Interfaces;

public interface IEventPublisher
{
    Task PublishAsync(string eventName, object eventData);
}
