using Microsoft.Extensions.DependencyInjection;
using SibersServices.Services;

namespace SibersServices;

public static class ServiceInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        
        return services;
    }
}