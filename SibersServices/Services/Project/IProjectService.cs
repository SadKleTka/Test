using SibersDataManager.Models;
using SibersDataManager.Models.Projects.Dto;

namespace SibersServices.Services;

public interface IProjectService
{
    public Task<ProjectToResponse> GetById(Guid id);
    public Task<IEnumerable<ProjectToResponse>> GetAll();
    
    public Task<Message> CreateProject(ProjectToCreate project);
    
    public Task<Message> UpdateProject(Guid id, ProjectToUpdate project);
    
    public Task<Message> DeleteProjectById(Guid id);
    
}