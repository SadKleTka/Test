using SibersDataManager.Models;
using SibersDataManager.Models.Employees;
using SibersDataManager.Models.Exceptions;
using SibersDataManager.Models.Employees.Dto;
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
            entity.Email);
    }

    public async Task<IEnumerable<EmployeeToResponse>> GetAll()
    {
        var entities = await _repository.GetAllAsync();
        return entities.Select(e 
            => new EmployeeToResponse(e.Id, e.Name, e.SecondName, e.ThirdName, e.Email));
    }

    public async Task<Message> CreateEmployee(EmployeeToCreate dto)
    {
        var entity = new EmployeeEntity(
            dto.FirstName, 
            dto.SecondName, 
            dto.ThirdName,
            dto.Email);
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
}