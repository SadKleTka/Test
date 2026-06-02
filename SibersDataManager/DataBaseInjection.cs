using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SibersDataManager.Data;
using SibersDataManager.Repository.ProjectRepository;

namespace SibersDataManager;

public static class DataBaseInjection
{
    public static IServiceCollection AddDataBase(this IServiceCollection services, string? connectionString)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));
        
        services.AddScoped<IProjectRepository, ProjectRepository>();
        
        return services;
    }
}