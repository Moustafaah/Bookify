using Application.Users.Events;

using Domain;
using Infrastructure;
using Infrastructure.Abstractions;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IEventHandler<UserCreatedDomainEvent>, UserCreatedEventHandler>();

        services.AddSingleton<IEnumerable<object>>(sp =>
        {
            var handlers = List<object>();
            return handlers.AddRange(sp.GetServices<IEventHandler<UserCreatedDomainEvent>>());
        });


        services.AddMediatR(sfg => sfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddInfrastructure(configuration);
        services.AddDomain();
        return services;
    }
}
