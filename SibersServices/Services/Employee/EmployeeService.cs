using SibersDataManager.Models;
using SibersDataManager.Models.Employees;
using SibersDataManager.Models.Exceptions;
using SibersDataManager.Models.Employees.Dto;
using SibersDataManager.Models.Projects;
using SibersDataManager.Models.Projects.Dto;
using SibersDataManager.Models.Tasks;
using SibersDataManager.Models.Tasks.Dto;
using SibersDataManager.Repository.Employee;

namespace SibersServices.Services.Employee;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _repository;

    public EmployeeService(IEmployeeRepository repository)
    {
        _repository = repository;
    }

    public async Task<EmployeeToResponse> GetById(Guid id)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null)
            throw new NotFoundException($"There is no Employee with id {id}");

        return new EmployeeToResponse(
            entity.Id, 
            entity.Name,
            entity.SecondName, 
            entity.ThirdName, 
            entity.Email,
            ProjectToResponse(entity.ManagedProjects),
            ProjectTaskToResponse(entity.WorkedTasks)
            );
    }

    public async Task<IEnumerable<EmployeeToResponse>> GetAll()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e 
            => new EmployeeToResponse(
                e.Id, e.Name, e.SecondName, e.ThirdName, e.Email,
                ProjectToResponse(e.ManagedProjects),
                ProjectTaskToResponse(e.WorkedTasks)));
    }

    public async Task<Message> CreateEmployee(EmployeeToCreate dto)
    {
        var entity = new EmployeeEntity(
            dto.FirstName, 
            dto.SecondName, 
            dto.ThirdName,
            dto.Email);
        
        var old = await _repository.GetByEmailAsync(dto.Email);
        if (old is not null)
            throw new BusinessValidationException($"There is already an Employee with email {dto.Email}");
        
        await _repository.PersistAsync(entity);
        return new Message("Employee created successfully", DateTime.UtcNow);
    }

    public async Task<Message> UpdateEmployee(Guid id, EmployeeToCreate dto)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null)
            throw new NotFoundException($"There is no Employee with id {id}");

        entity.Name = dto.FirstName;
        entity.SecondName = dto.SecondName;
        entity.ThirdName = dto.ThirdName;
        entity.Email = dto.Email;

        await _repository.UpdateAsync(entity);
        return new Message("Employee updated successfully", DateTime.UtcNow);
    }

    public async Task<Message> DeleteEmployeeById(Guid id)
    {
        var entity = await _repository.FindAsync(id);
        if (entity is null)
            throw new NotFoundException($"There is no Employee with id {id}");

        await _repository.DeleteAsync(entity);
        return new Message("Employee deleted successfully", DateTime.UtcNow);
    }

    private static List<ProjectToResponse> ProjectToResponse(ICollection<ProjectEntity> entities)
    {
        List<ProjectToResponse> list = new List<ProjectToResponse>();
        foreach (var entity in entities)
        {
            list.Add(new ProjectToResponse(
                entity.Id,
                entity.Name,
                entity.CustomerCompany, 
                entity.WorkerCompany, 
                entity.StartDate,
                entity.EndDate,
                entity.Priority, 
                entity.ManagerId));
        }

        return list;
    }

    private static List<ProjectTaskToResponse> ProjectTaskToResponse(ICollection<ProjectTaskEntity> entities)
    {
        List<ProjectTaskToResponse> list = new List<ProjectTaskToResponse>();
        foreach (var entity in entities)
        {
            list.Add(new ProjectTaskToResponse(
                entity.Id, 
                entity.Name, 
                entity.AuthorId,
                entity.WorkerId, 
                entity.Status, 
                entity.Comment, 
                entity.Priority, 
                entity.ProjectId));
        }
        return list;
    }
}