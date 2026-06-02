using Microsoft.Extensions.DependencyInjection;
using SibersServices.Services;
using SibersServices.Services.Employee;
using SibersServices.Services.Task;

namespace SibersServices;

public static class ServiceInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<ITaskService, TaskService>();
        
        return services;
    }
}