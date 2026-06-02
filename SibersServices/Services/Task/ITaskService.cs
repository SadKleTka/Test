using SibersDataManager.Models;
using SibersDataManager.Models.Tasks.Dto;

namespace SibersServices.Services.Task;

public interface ITaskService
{
    public Task<ProjectTaskToResponse> GetById(Guid id);

    public Task<IEnumerable<ProjectTaskToResponse>> GetAll();

    public Task<Message> CreateTask(ProjectTaskToCreate dto);

    public Task<Message> UpdateTask(Guid id, ProjectTaskToUpdate dto);

    public Task<Message> DeleteTaskById(Guid id);
    
}