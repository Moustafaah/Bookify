using Domain.Abstraction;
using Domain.Users.Events;

using Infrastructure.EventHandlers.Users;
using Infrastructure.Outbox;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OutboxOptions>(o => configuration.GetSection(nameof(OutboxOptions)));
        services.AddHostedService<ProcessOutboxMessages>();
        services.AddSingleton<IEventHandler<UserCreatedDomainEvent>, UserCreatedEventHandler>();

        services.AddSingleton<IEnumerable<object>>(sp =>
        {
            var handlers = List<object>();
            return handlers.AddRange(sp.GetServices<IEventHandler<UserCreatedDomainEvent>>());
        });
        return services;
    }
}
