using SibersDataManager.Models;
using SibersDataManager.Models.Exceptions;
using SibersDataManager.Models.Projects;
using SibersDataManager.Models.Projects.Dto;
using SibersDataManager.Repository.ProjectRepository;

namespace SibersServices.Services;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _repository;

    public ProjectService(IProjectRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<ProjectToResponse>> GetFiltered(
        uint? priority,
        string? sortBy)
    {
        var projects =
            await _repository.GetFilteredAsync(
                priority,
                sortBy);

        return AllMapper(projects);
    }
    
    public async Task<ProjectToResponse> GetById(Guid id)
    {
        var project = await _repository.FindAsync(id);
        if (project is null)
            throw new NotFoundException($"There is no Projects with id {id}");
        
        return ToResponse(project);
    }

    public async Task<IEnumerable<ProjectToResponse>> GetAll()
    {
        var projects = await _repository.GetAllAsync();
        
        return AllMapper(projects);
    }

    public async Task<Message> CreateProject(ProjectToCreate project)
    {
        await CreateProjectEntity(project);
        return new Message("Project created successfully", DateTime.UtcNow);
    }

    public async Task<ProjectToResponse> CreateProjectAndReturn(ProjectToCreate project)
    {
        var entity = await CreateProjectEntity(project);
        return ToResponse(entity);
    }

    private async Task<ProjectEntity> CreateProjectEntity(ProjectToCreate project)
    {
        if (project.EndDate.HasValue && project.EndDate < project.StartDate)
            throw new BusinessValidationException("EndDate must be after StartDate");

        var newProject = new ProjectEntity(
            project.Name,
            project.CustomerCompany,
            project.WorkerCompany,
            project.StartDate,
            project.Priority,
            project.EndDate,
            project.ManagerId);

        await _repository.PersistAsync(newProject);
        return newProject;
    }

    public async Task<Message> UpdateProject(Guid id, ProjectToUpdate project)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null)
            throw new NotFoundException($"There is no Projects with id {id}");
        
        if (project.EndDate.HasValue && project.EndDate < entity.StartDate)
            throw new BusinessValidationException("New EndDate must be after StartDate");
        
        entity.Name = project.Name;
        entity.CustomerCompany = project.CustomerCompany;
        entity.WorkerCompany = project.WorkerCompany;
        entity.Priority = project.Priority;
        entity.EndDate = project.EndDate;
        entity.ManagerId = project.ManagerId;
        
        await _repository.UpdateAsync(entity);
        
        return new Message("Project updated successfully", DateTime.UtcNow);
    }

    public async Task<Message> DeleteProjectById(Guid id)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null)
            throw new NotFoundException($"There is no Projects with id {id}");
        
        await _repository.DeleteAsync(entity);
        return new Message("Project deleted successfully", DateTime.UtcNow);
    }


    private static IEnumerable<ProjectToResponse> AllMapper(IEnumerable<ProjectEntity> entities)
    {
        return entities.Select(x => ToResponse(x));
    }
    private static ProjectToResponse ToResponse(ProjectEntity entity)
    {
        return new ProjectToResponse(
            entity.Id, 
            entity.Name, 
            entity.CustomerCompany, 
            entity.WorkerCompany, 
            entity.StartDate, 
            entity.EndDate,
            entity.Priority,
            entity.ManagerId);
    }
}