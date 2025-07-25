using Microsoft.Extensions.DependencyInjection;

namespace Domain;
public static class DependencyInjection
{
    public static IServiceCollection AddDomain(this IServiceCollection services)

    {
        services.AddSingleton<DateTimeUtc>(_ => () => DateTime.UtcNow);



        return services;
    }
}
