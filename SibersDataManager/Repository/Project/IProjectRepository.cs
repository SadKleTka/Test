using SibersDataManager.Models.Projects;

namespace SibersDataManager.Repository.ProjectRepository;

public interface IProjectRepository
{
    public Task PersistAsync(ProjectEntity entity);

    public Task<IEnumerable<ProjectEntity>> GetAllAsync();

    public Task<ProjectEntity?> FindAsync(Guid id);

    public Task UpdateAsync(ProjectEntity entity);

    public Task DeleteAsync(ProjectEntity entity);
}