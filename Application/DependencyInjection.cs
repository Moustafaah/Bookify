using Domain;

using Infrastructure;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application;
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddMediatR(sfg => sfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        services.AddInfrastructure(configuration);
        services.AddDomain();
        return services;
    }
}
