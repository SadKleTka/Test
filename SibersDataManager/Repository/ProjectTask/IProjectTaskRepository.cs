using SibersDataManager.Models.Roles;
using SibersDataManager.Models.Tasks;

namespace SibersDataManager.Repository.ProjectTask;

public interface IProjectTaskRepository
{
    
    Task<IEnumerable<ProjectTaskEntity>> GetFilteredAsync(TaskStatusEnum? status, string? sortBy);
    Task<ProjectTaskEntity?> FindAsync(Guid id);
    
    Task<IEnumerable<ProjectTaskEntity>> GetAllAsync();
    
    Task PersistAsync(ProjectTaskEntity entity);
    
    Task UpdateAsync(ProjectTaskEntity entity);
    
    Task DeleteAsync(ProjectTaskEntity entity);
}