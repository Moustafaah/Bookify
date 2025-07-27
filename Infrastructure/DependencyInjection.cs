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

        return services;
    }
}
