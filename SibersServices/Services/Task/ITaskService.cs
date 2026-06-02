using SibersDataManager.Models;
using SibersDataManager.Models.Roles;
using SibersDataManager.Models.Tasks.Dto;

namespace SibersServices.Services.Task;

public interface ITaskService
{
    
    Task<IEnumerable<ProjectTaskToResponse>> GetFiltered(TaskStatusEnum? status, string? sortBy);
    public Task<ProjectTaskToResponse> GetById(Guid id);

    public Task<IEnumerable<ProjectTaskToResponse>> GetAll();

    public Task<Message> CreateTask(ProjectTaskToCreate dto);

    public Task<Message> UpdateTask(Guid id, ProjectTaskToUpdate dto);

    public Task<Message> DeleteTaskById(Guid id);
    
}