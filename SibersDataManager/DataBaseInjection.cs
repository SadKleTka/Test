using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SibersDataManager.Data;
using SibersDataManager.Repository.Employee;
using SibersDataManager.Repository.ProjectRepository;
using SibersDataManager.Repository.ProjectTask;

namespace SibersDataManager;

public static class DataBaseInjection
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IProjectTaskRepository, ProjectTaskRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        
        return services;
    }
}