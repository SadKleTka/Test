using SibersDataManager.Models;
using SibersDataManager.Models.Exceptions;
using SibersDataManager.Models.Tasks;
using SibersDataManager.Models.Tasks.Dto;
using SibersDataManager.Repository.ProjectTask;

namespace SibersServices.Services.Task;

public class TaskService : ITaskService
{
    private readonly IProjectTaskRepository _repository;

    public TaskService(IProjectTaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<ProjectTaskToResponse> GetById(Guid id)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null)
            throw new NotFoundException($"There is no Task with id {id}");

        return new ProjectTaskToResponse(
            entity.Id, 
            entity.Name,
            entity.AuthorId,
            entity.WorkerId,
            entity.Status,
            entity.Comment,
            entity.Priority,
            entity.ProjectId);
    }

    public async Task<IEnumerable<ProjectTaskToResponse>> GetAll()
    {
        var entities = await _repository.GetAllAsync();

        return entities.Select(e 
            => new ProjectTaskToResponse(
                e.Id, e.Name, e.AuthorId, e.WorkerId, e.Status, e.Comment, e.Priority, e.ProjectId));
    }

    public async Task<Message> CreateTask(ProjectTaskToCreate dto)
    {
        var entity = new ProjectTaskEntity(
            dto.Name,
            dto.AuthorId,
            dto.ExecutorId, 
            dto.Status,
            dto.Comment, 
            dto.Priority,
            dto.ProjectId);
        await _repository.PersistAsync(entity);
        return new Message("Task created successfully", DateTime.UtcNow);
    }

    public async Task<Message> UpdateTask(Guid id, ProjectTaskToUpdate dto)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null)
            throw new NotFoundException($"There is no Task with id {id}");

        entity.Name = dto.Name;
        entity.WorkerId = dto.ExecutorId; 
        entity.Status = dto.Status;
        entity.Comment = dto.Comment;
        entity.Priority = dto.Priority;

        await _repository.UpdateAsync(entity);
        return new Message("Task updated successfully", DateTime.UtcNow);
    }

    public async Task<Message> DeleteTaskById(Guid id)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null)
            throw new NotFoundException($"There is no Task with id {id}");

        await _repository.DeleteAsync(entity);
        return new Message("Task deleted successfully", DateTime.UtcNow);
    }
}